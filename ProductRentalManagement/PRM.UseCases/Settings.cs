﻿using Microsoft.Extensions.DependencyInjection;
using PRM.UseCases.Products;
using PRM.UseCases.Renters;
using PRM.UseCases.Rents;
using PRM.UseCases.Rents.RentProducts;

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
                .AddTransient<IRentProduct, RentProduct>()
                
                // Renters
                .AddTransient<IRenterUseCasesReadOnlyInteractor, RenterUseCasesReadOnlyInteractor>()
                .AddTransient<IRenterUseCasesManipulationInteractor, RenterUseCasesManipulationInteractor>()
                ;
            
            
            
            return services;
        }
    }
}