using Microsoft.EntityFrameworkCore;
using WorkFlow360.Application.Common.Interface;
using WorkFlow360.Application.Common.Messaging;
using WorkFlow360.Application.Common.Results;
using WorkFlow360.Domain.Entities;

namespace WorkFlow360.Application.Authentication.Register
{
    public sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IApplicationDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterCommandHandler(IApplicationDbContext dbContext, IPasswordHasher passwordHasher)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<RegisterResponse>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            var normalizedEmail = command.Email.Trim().ToLowerInvariant();

            var emailExists = await _dbContext.Users.AnyAsync(x => x.Email == normalizedEmail, cancellationToken);

            if (emailExists)
            {
                return Result<RegisterResponse>.Failure(Error.Conflict("User.EmailAlreadyExists", "A user with this email address already exists."));
            }

            var passwordHash = _passwordHasher.Hash(command.Password);
            var user = User.Create(normalizedEmail, command.FirstName, command.LastName, passwordHash, "User");

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = new RegisterResponse(user.Id, user.Email, user.FirstName, user.LastName);
            return Result<RegisterResponse>.Success(response);
        }
    }
}
