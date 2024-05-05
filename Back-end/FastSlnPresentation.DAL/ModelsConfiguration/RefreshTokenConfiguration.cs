using FastSlnPresentation.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastSlnPresentation.DAL.ModelsConfiguration
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> entity)
        {
            entity.HasKey(rt => rt.Id); // Set the primary key

            entity.ToTable("refresh_tokens"); // Set the table name

            // Configure properties
            entity.Property(rt => rt.Id).HasColumnName("id");
            entity.Property(rt => rt.Token).HasMaxLength(256).IsRequired().HasColumnName("token");
            entity.Property(rt => rt.UserId).IsRequired().HasColumnName("user_id");
            entity
                .Property(rt => rt.CreatedAt)
                .IsRequired()
                .HasColumnName("created_at")
                .HasColumnType("timestamp");

            entity
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
