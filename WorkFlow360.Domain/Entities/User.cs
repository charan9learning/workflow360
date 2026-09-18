using WorkFlow360.Domain.Common;

namespace WorkFlow360.Domain.Entities
{
    public sealed class User : Entity
    {
        private User(Guid id, string email, string firstName, string lastName, string passwordHash, string role, DateTime createdAtUtc) : base(id)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            PasswordHash = passwordHash;
            Role = role;
            CreatedAtUtc = createdAtUtc;
            IsActive = true;
        }

        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string PasswordHash { get; private set; }
        public string Role { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public bool IsActive { get; private set; }

        public static User Create(string email, string firstName, string lastName, string passwordHash, string role)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.", nameof(email));
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required.", nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required.", nameof(lastName));
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Password hash is required.", nameof(passwordHash));
            if (string.IsNullOrWhiteSpace(role)) throw new ArgumentException("Role is required.", nameof(role));

            return new User(Guid.NewGuid(), email.Trim().ToLowerInvariant(), firstName.Trim(), lastName.Trim(), passwordHash, role, DateTime.UtcNow);
        }
    }
}
