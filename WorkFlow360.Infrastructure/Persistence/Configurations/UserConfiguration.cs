using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkFlow360.Domain.Entities;

namespace WorkFlow360.Infrastructure.Persistence.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email).HasMaxLength(320).IsRequired();
            builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();
            builder.Property(x => x.Role).HasMaxLength(50).IsRequired();
            builder.Property(x => x.CreatedAtUtc).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();

            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
