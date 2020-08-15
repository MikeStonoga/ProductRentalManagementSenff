using Microsoft.Extensions.DependencyInjection;
using PRM.InterfaceAdapters.Controllers.Products;
using PRM.InterfaceAdapters.Controllers.Rents;

namespace PRM.InterfaceAdapters.Controllers
{
    public static class ControllersSettings
    {
        public static IServiceCollection AddControllersTransients(this IServiceCollection services)
        {
            services
                .AddTransient<IProductReadOnlyController, ProductReadOnlyController>()
                .AddTransient<IProductManipulationController, ProductManipulationController>()
                .AddTransient<IRentReadOnlyController, RentReadOnlyController>()
                .AddTransient<IRentManipulationController, RentManipulationController>();
            return services;
        }
    }
}