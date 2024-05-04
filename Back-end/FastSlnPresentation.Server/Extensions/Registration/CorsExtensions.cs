namespace FastSlnPresentation.Server.Extensions
{
    public static class CorsExtensions
    {
        public static void AddDefaultCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
        }
    }
}
