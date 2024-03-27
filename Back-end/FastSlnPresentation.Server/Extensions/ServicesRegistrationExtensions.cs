using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastSlnPresentation.BLL.Contracts;
using FastSlnPresentation.BLL.Services;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace FastSlnPresentation.Server.Extensions
{
    public static class ServicesRegistrationExtensions
    {
        public static IServiceCollection AddCustomControllers(this IServiceCollection services)
        {
            services.AddTransient<IIdService, IdService>();

            return services;
        }
    }
}
