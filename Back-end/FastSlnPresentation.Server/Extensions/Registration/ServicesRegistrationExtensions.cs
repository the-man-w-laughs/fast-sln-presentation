using FastSlnPresentation.BLL.Contracts;
using FastSlnPresentation.BLL.Services;
using FastSlnPresentation.BLL.Services.DBServices;

namespace FastSlnPresentation.Server.Extensions
{
    public static class ServicesRegistrationExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // register hard services
            services.AddTransient<IIdService, IdService>();
            services.AddTransient<IMethodAnalysisService, MethodAnalysisService>();
            services.AddTransient<IClassAnalysisService, ClassAnalysisService>();

            // register DB services
            services.AddTransient<UserService>();
            services.AddTransient<SubscriptionService>();
            services.AddTransient<PlanService>();

            return services;
        }
    }
}
