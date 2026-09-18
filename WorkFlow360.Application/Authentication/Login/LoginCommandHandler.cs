using Microsoft.EntityFrameworkCore;
using WorkFlow360.Application.Common.Interface;
using WorkFlow360.Application.Common.Messaging;
using WorkFlow360.Application.Common.Results;

namespace WorkFlow360.Application.Authentication.Login
{
    public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(IApplicationDbContext dbContext, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var normalizedEmail = command.Email.Trim().ToLowerInvariant();

            var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken);

            if (user is null || !_passwordHasher.Verify(command.Password, user.PasswordHash))
            {
                return Result<LoginResponse>.Failure(Error.Unauthorized("Authentication.InvalidCredentials", "Invalid email or password."));
            }

            if (!user.IsActive)
            {
                return Result<LoginResponse>.Failure(Error.Unauthorized("Authentication.UserInactive", "The user account is inactive."));
            }

            var accessToken = _jwtTokenGenerator.GenerateToken(user);

            var response = new LoginResponse(user.Id, user.Email, user.FirstName, user.LastName, user.Role, accessToken);

            return Result<LoginResponse>.Success(response);
        }
    }
}
