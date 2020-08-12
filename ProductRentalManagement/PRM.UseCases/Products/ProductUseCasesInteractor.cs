using System.Threading.Tasks;
using PRM.Domain.Products;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;
using PRM.UseCases.Products.GetProductRentPrice;

namespace PRM.UseCases.Products
{
    public interface IProductUseCasesReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<Product>
    {
        Task<UseCaseResult<decimal>> GetProductRentPrice(GetProductRentPriceRequirement rentPriceRequirement);
    }
        
    public class ProductUseCasesReadOnlyInteractor : BaseUseCaseReadOnlyInteractor<Product>, IProductUseCasesReadOnlyInteractor
    {
        private readonly IGetProductRentPrice _getProductRentPrice; 
        
        public ProductUseCasesReadOnlyInteractor(IReadOnlyPersistenceGateway<Product> baseReadOnlyPersistenceGateway, IGetProductRentPrice getProductRentPrice) : base(baseReadOnlyPersistenceGateway)
        {
            _getProductRentPrice = getProductRentPrice;
        }

        public async Task<UseCaseResult<decimal>> GetProductRentPrice(GetProductRentPriceRequirement rentPriceRequirement)
        {
            var productRentPrice = await _getProductRentPrice.Execute(rentPriceRequirement);
            return productRentPrice.Success ? UseCasesResponses.UseCaseSuccessfullyExecutedResponse(productRentPrice.Result, "Success") : UseCasesResponses.PersistenceErrorResponse(productRentPrice.Result, productRentPrice.Message);
        }
    }

    public interface IProductUseCasesManipulationInteractor : IBaseUseCaseManipulationInteractor<Product>, IProductUseCasesReadOnlyInteractor
    {
        
    }

    public class ProductUseCasesManipulationInteractor : BaseUseCaseManipulationInteractor<Product, IProductUseCasesReadOnlyInteractor>, IProductUseCasesManipulationInteractor
    {
        
        public ProductUseCasesManipulationInteractor(IManipulationPersistenceGateway<Product> basePersistenceGateway, IProductUseCasesReadOnlyInteractor subjectUseCasesReadOnlyInteractor) : base(basePersistenceGateway, subjectUseCasesReadOnlyInteractor)
        {
        }

        public async Task<UseCaseResult<decimal>> GetProductRentPrice(GetProductRentPriceRequirement rentPriceRequirement)
        {
            return await UseCasesReadOnlyInteractor.GetProductRentPrice(rentPriceRequirement);
        }
    }
}