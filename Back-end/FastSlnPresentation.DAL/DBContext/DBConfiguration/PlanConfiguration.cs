using FastSlnPresentation.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastSlnPresentation.BLL.DBConfiguration
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            var plans = new List<Plan>
            {
                new Plan
                {
                    Id = 1,
                    Name = "Стандарт",
                    Price = 17,
                    Description = "Подписка на месяц",
                    Duration = 30
                },
                new Plan
                {
                    Id = 2,
                    Name = "Стандарт годовая",
                    Price = 204,
                    Description = "Подписка на год",
                    Duration = 365
                },
            };

            builder.HasData(plans);
        }
    }
}
