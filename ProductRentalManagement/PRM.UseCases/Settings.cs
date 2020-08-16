using Microsoft.Extensions.DependencyInjection;
using PRM.UseCases.Products;
using PRM.UseCases.Renters;
using PRM.UseCases.Rents;
using PRM.UseCases.Rents.FinishRents;
using PRM.UseCases.Rents.GetRentForecastPrices;
using PRM.UseCases.Rents.RentProducts;
using PRM.UseCases.Rents.Validations.Requirements.Rents;

namespace PRM.UseCases
{
    public static class UseCasesSettings
    {
     
        public static IServiceCollection AddUseCasesTransients(this IServiceCollection services)
        {
            services
                //Products
                .AddTransient<IProductUseCasesReadOnlyInteractor, ProductUseCasesReadOnlyInteractor>()
                .AddTransient<IProductUseCasesManipulationInteractor, ProductUseCasesManipulationInteractor>()
                
                //Rents
                .AddTransient<IRentUseCasesReadOnlyInteractor, RentUseCasesReadOnlyInteractor>()
                .AddTransient<IRentUseCasesManipulationInteractor, RentUseCasesManipulationInteractor>()
                .AddTransient<IGetRentForecastPrice, GetRentForecastPrice>()
                .AddTransient<IRentProducts, RentProducts>()
                .AddTransient<IFinishRent, FinishRent>()
                .AddTransient<IValidateRentRequirement, ValidateRentRequirement>()
                
                // Renters
                .AddTransient<IRenterUseCasesReadOnlyInteractor, RenterUseCasesReadOnlyInteractor>()
                .AddTransient<IRenterUseCasesManipulationInteractor, RenterUseCasesManipulationInteractor>()
                ;
            
            
            
            return services;
        }
    }
}