using System.Reflection;
using FastSlnPresentation.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FastSlnPresentation.DAL.DBContext
{
    public partial class FastSlnPresentationDbContext : DbContext
    {
        public DbSet<Plan> Plans { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<User> Users { get; set; }

        public FastSlnPresentationDbContext(DbContextOptions<FastSlnPresentationDbContext> options)
            : base(options)
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            builder.ApplyConfigurationsFromAssembly(executingAssembly);
        }
    }
}
