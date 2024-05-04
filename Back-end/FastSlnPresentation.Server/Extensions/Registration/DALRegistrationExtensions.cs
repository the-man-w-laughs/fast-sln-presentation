using FastSlnPresentation.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace FastSlnPresentation.Server.Extensions
{
    public static class DALRegistrationExtensions
    {
        public static void RegisterDALDependencies(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<FastSlnPresentationDbContext>(builder =>
            {
                var connectionString = configuration.GetConnectionString("Default");

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException("Connection string cannot be empty.");
                }

                builder.UseLazyLoadingProxies();
                builder.UseNpgsql(
                    connectionString,
                    options => options.MigrationsAssembly("FastSlnPresentation.Server")
                );
            });
        }
    }
}
