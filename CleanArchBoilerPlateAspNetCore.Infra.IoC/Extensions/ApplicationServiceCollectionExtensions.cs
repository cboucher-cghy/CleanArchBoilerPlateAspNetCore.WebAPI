using CleanArchBoilerPlateAspNetCore.Core.Application.Interfaces;
using CleanArchBoilerPlateAspNetCore.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CleanArchBoilerPlateAspNetCore.Infra.IoC.Extensions
{
    internal static class ApplicationServiceCollectionExtensions
    {
        internal static void RegisterApplicationServices(this IServiceCollection services)
        {
            //CleanArchBoilerPlateAspNetCore.Core.Application
            services.AddTransient<IFeatureService, FeatureService>();
            services.AddTransient<IUserService, UserService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
