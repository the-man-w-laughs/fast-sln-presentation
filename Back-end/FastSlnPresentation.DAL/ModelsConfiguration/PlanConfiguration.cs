using FastSlnPresentation.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastSlnPresentation.DAL.ModelsConfiguration
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("plans");

            entity.Property(e => e.Id).HasColumnName("plan_id");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.Price).HasColumnType("decimal(10,2)").HasColumnName("price");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity
                .Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(1024)
                .IsRequired(false);
        }
    }
}
