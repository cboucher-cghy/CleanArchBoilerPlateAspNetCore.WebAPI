using CleanArchBoilerPlateAspNetCore.Core.Application.Interfaces;
using CleanArchBoilerPlateAspNetCore.Core.Domain.Interfaces;
using CleanArchBoilerPlateAspNetCore.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchBoilerPlateAspNetCore.Infra.IoC.Extensions
{
    internal static class DomainServiceCollectionExtensions
    {
        internal static void RegisterDomainServices(this IServiceCollection services)
        {
            //CleanArchBoilerPlateAspNetCore.Core.Domain.Interfaces | CleanArchBoilerPlateAspNetCore.Infra.Data.Repositories
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IFeatureRepository, FeatureRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
        }
    }
}
