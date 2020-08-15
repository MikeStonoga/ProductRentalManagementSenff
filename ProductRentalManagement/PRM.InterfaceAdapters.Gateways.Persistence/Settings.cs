using System;
using Microsoft.Extensions.DependencyInjection;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;

namespace PRM.InterfaceAdapters.Gateways.Persistence
{
    public static class PersistenceSettings
    {
        public static IServiceCollection AddPersistenceTransients(this IServiceCollection services, Type readOnlyImplementation, Type readOnlyInterface,Type manipulationImplementation)
        {
            services
                .AddTransient(typeof(IReadOnlyPersistenceGateway<>), readOnlyImplementation)
                .AddTransient(readOnlyInterface, readOnlyImplementation)
                .AddTransient(typeof(IManipulationPersistenceGateway<>), manipulationImplementation);
            
            return services;
        }
        
    }
}