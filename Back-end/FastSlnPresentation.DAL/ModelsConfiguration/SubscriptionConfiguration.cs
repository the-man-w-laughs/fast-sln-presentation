using FastSlnPresentation.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastSlnPresentation.DAL.ModelsConfiguration
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("subscriptions");

            entity.Property(e => e.Id).HasColumnName("subscription_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.PlanId).HasColumnName("plan_id");
            entity
                .Property(e => e.StartDate)
                .HasColumnType("timestamp")
                .HasColumnName("start_date");
            entity.Property(e => e.EndDate).HasColumnType("timestamp").HasColumnName("end_date");
            entity
                .Property(e => e.CreatedAt)
                .HasColumnType("timestamp")
                .HasColumnName("created_at");

            entity
                .HasOne(d => d.User)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_subscriptions_users");

            entity
                .HasOne(d => d.Plan)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_subscriptions_plans");
        }
    }
}
