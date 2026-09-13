using WorkFlow360.Domain.Common;

namespace WorkFlow360.Domain.Entities
{
    public sealed class Project : Entity
    {
        private Project(
            Guid id,
            string name,
            string? description,
            DateTime createdAtUtc)
            : base(id)
        {
            Name = name;
            Description = description;
            CreatedAtUtc = createdAtUtc;
        }

        public string Name { get; private set; }

        public string? Description { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }

        public DateTime? UpdatedAtUtc { get; private set; }

        public static Project Create(
            string name,
            string? description)
        {
            ValidateName(name);

            return new Project(
                Guid.NewGuid(),
                name.Trim(),
                description?.Trim(),
                DateTime.UtcNow);
        }

        public void Update(
            string name,
            string? description)
        {
            ValidateName(name);

            Name = name.Trim();
            Description = description?.Trim();
            UpdatedAtUtc = DateTime.UtcNow;
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    "Project name is required.",
                    nameof(name));
            }

            if (name.Length > 150)
            {
                throw new ArgumentException(
                    "Project name cannot exceed 150 characters.",
                    nameof(name));
            }
        }
    }
}
