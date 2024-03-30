using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Octokit;
using FastSlnPresentation.BLL.Contracts;
using FastSlnPresentation.BLL.Services;
using FastSlnPresentation.BLL.SyntaxWalkers;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace FastSlnPresentation.Server.Extensions
{
    public static class ServicesRegistrationExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IIdService, IdService>();
            services.AddTransient<IMethodAnalysisService, MethodAnalysisService>();
            services.AddTransient<IClassAnalysisService, ClassAnalysisService>();

            return services;
        }
    }
}
