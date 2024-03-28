using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
