using FastSlnPresentation.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastSlnPresentation.DAL.ModelsConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("role_id");
            entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("name");
        }
    }
}
