using System.Reflection;
using FastSlnPresentation.Server.Security;
using Microsoft.OpenApi.Models;

namespace FastSlnPresentation.Server.Extensions
{
    public static class SwaggerExtensions
    {
        public static void ConfigureSwagger(this IServiceCollection services, IConfiguration config)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc(
                    "v1",
                    new OpenApiInfo { Title = "fast-sln-presentation API", Version = "v1" }
                );
                option.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = AuthOptions.AuthenticationScheme
                    }
                );
                option.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = AuthOptions.AuthenticationScheme
                                }
                            },
                            new string[] { }
                        }
                    }
                );
            });
        }
    }
}
