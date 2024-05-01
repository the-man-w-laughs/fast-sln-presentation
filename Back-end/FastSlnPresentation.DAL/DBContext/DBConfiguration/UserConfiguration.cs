using FastSlnPresentation.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Services.Static;

namespace FastSlnPresentation.BLL.DBConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var (passwordHash, salt) = PasswordHashingService.HashPassword("admin");
            var admin = new User
            {
                Id = 1,
                Name = "Admin",
                Email = "admin@the.best",
                PasswordHash = passwordHash,
                Salt = salt,
                RoleId = 1,
                CreatedAt = DateTime.Now,
            };

            builder.HasData(admin);
        }
    }
}
