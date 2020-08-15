using Microsoft.Extensions.DependencyInjection;
using PRM.UseCases.Products;
using PRM.UseCases.Rents;
using PRM.UseCases.Rents.RentProducts;

namespace PRM.UseCases
{
    public static class UseCasesSettings
    {
     
        public static IServiceCollection AddUseCasesTransients(this IServiceCollection services)
        {
            services
                .AddTransient<IProductUseCasesReadOnlyInteractor, ProductUseCasesReadOnlyInteractor>()
                .AddTransient<IProductUseCasesManipulationInteractor, ProductUseCasesManipulationInteractor>()
                .AddTransient<IRentUseCasesReadOnlyInteractor, RentUseCasesReadOnlyInteractor>()
                .AddTransient<IRentUseCasesManipulationInteractor, RentUseCasesManipulationInteractor>()
                .AddTransient<IRentProduct, RentProduct>();
            
            return services;
        }
    }
}