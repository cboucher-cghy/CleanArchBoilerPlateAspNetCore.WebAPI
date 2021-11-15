using Microsoft.Extensions.DependencyInjection;
using CleanArchBoilerPlateAspNetCore.Infra.IoC.Extensions;

namespace CleanArchBoilerPlateAspNetCore.Infra.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.RegisterApplicationServices();
            services.RegisterDomainServices();
        }
    }
}
