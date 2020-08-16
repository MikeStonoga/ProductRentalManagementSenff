using System;
using System.Threading.Tasks;
using PRM.Domain.Products;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;
using PRM.UseCases.Products.CheckAvailabilities;
using PRM.UseCases.Products.CheckProductAvailabilities;

namespace PRM.UseCases.Products
{
    public interface IProductUseCasesReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<Product>
    {
        Task<UseCaseResult<CheckProductAvailabilityResult>> CheckProductAvailability(Guid productId);
    }
        
    public class ProductUseCasesReadOnlyInteractor : BaseUseCaseReadOnlyInteractor<Product>, IProductUseCasesReadOnlyInteractor
    {
        private readonly ICheckProductAvailability _checkProductAvailability;
        
        public ProductUseCasesReadOnlyInteractor(IReadOnlyPersistenceGateway<Product> readOnlyPersistenceGateway, ICheckProductAvailability checkProductAvailability) : base(readOnlyPersistenceGateway)
        {
            _checkProductAvailability = checkProductAvailability;
        }

        public async Task<UseCaseResult<CheckProductAvailabilityResult>> CheckProductAvailability(Guid productId)
        {
            return await UseCasesResponses.GetUseCaseExecutionResponse<ICheckProductAvailability, Guid, CheckProductAvailabilityResult>(_checkProductAvailability, productId);
        }
    }

    public interface IProductUseCasesManipulationInteractor : IBaseUseCaseManipulationInteractor<Product>, IProductUseCasesReadOnlyInteractor
    {

    }

    public class ProductUseCasesManipulationInteractor : BaseUseCaseManipulationInteractor<Product, IProductUseCasesReadOnlyInteractor>, IProductUseCasesManipulationInteractor
    {
        public ProductUseCasesManipulationInteractor(IManipulationPersistenceGateway<Product> persistenceGateway, IProductUseCasesReadOnlyInteractor useCasesReadOnlyInteractor, IManipulationPersistenceGateway<ProductRentalHistory> productRentalHistories) : base(persistenceGateway, useCasesReadOnlyInteractor)
        {
        }

        public async Task<UseCaseResult<CheckProductAvailabilityResult>> CheckProductAvailability(Guid productId)
        {
            return await UseCasesReadOnlyInteractor.CheckProductAvailability(productId);
        }
    }
}