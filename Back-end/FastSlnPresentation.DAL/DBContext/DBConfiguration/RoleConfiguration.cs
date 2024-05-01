using FastSlnPresentation.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastSlnPresentation.BLL.DBConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            var holidays = new List<Role>
            {
                new Role { Id = 1, Name = "Админ", },
                new Role { Id = 2, Name = "Клиент", },
            };

            builder.HasData(holidays);
        }
    }
}
