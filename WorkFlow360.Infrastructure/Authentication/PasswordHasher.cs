using Microsoft.AspNetCore.Identity;
using WorkFlow360.Application.Common.Interface;

namespace WorkFlow360.Infrastructure.Authentication
{
    internal sealed class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<object> _passwordHasher = new();
        public string Hash(string password)
        {
            return _passwordHasher.HashPassword(null!, password);
        }

        public bool Verify(string password, string passwordHash)
        {
            var result = _passwordHasher.VerifyHashedPassword(null!, passwordHash, password);
            return result != PasswordVerificationResult.Failed;
        }
    }
}
