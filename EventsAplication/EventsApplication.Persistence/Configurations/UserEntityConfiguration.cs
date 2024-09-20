using EventsApplication.Domain.Enums;
using EventsApplication.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventsApplication.Persistence.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasMany(u => u.EventSubscriptions)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(u => u.UserRole)
                .HasConversion(
                    v => v.ToString(),
                    v => (Role)Enum.Parse(typeof(Role), v));

            builder.HasData(
                new UserEntity
                {
                    Id = Guid.Parse("43130ccf-faf1-4445-8f99-a54a1e661f5d"),
                    Email = "Admin@gmail.com",
                    PasswordHash = "$2a$11$0fz1SsBAF8ZIC1nNAcJ0MOnrU8gQp1.CpP5oz5YD5OShgobb2wGAq",
                    FirstName = "Admin",
                    LastName = "Admin",
                    UserRole = Role.Admin,
                    IsActivated = true
                });
        }
    }
}
