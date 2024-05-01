using FastSlnPresentation.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastSlnPresentation.DAL.ModelsConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("users");

            entity.HasIndex(e => e.Email).IsUnique();

            entity.Property(e => e.Id).HasColumnName("user_id");
            entity.Property(e => e.Name).HasMaxLength(100).HasColumnName("name");
            entity.Property(e => e.PasswordHash).HasMaxLength(256).HasColumnName("passwork_hash");
            entity.Property(e => e.Salt).HasMaxLength(256).HasColumnName("salt");
            entity.Property(e => e.Email).HasMaxLength(150).HasColumnName("email");
            entity
                .Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity
                .HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_users_roles");
        }
    }
}
