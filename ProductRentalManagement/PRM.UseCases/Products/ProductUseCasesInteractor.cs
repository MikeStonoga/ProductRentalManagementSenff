using System.Threading.Tasks;
using PRM.Domain.Products;
using PRM.Domain.Products.Rents.Dtos;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;
using PRM.UseCases.Products.FinishRent;
using PRM.UseCases.Products.GetProductRentPrice;
using PRM.UseCases.Products.RentProduct;

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
            
            return productRentPrice.Success 
                ? UseCasesResponses.SuccessfullyExecutedResponse(productRentPrice.Result, productRentPrice.Message) 
                : UseCasesResponses.PersistenceErrorResponse(productRentPrice.Result, productRentPrice.Message);
        }
    }

    public interface IProductUseCasesManipulationInteractor : IBaseUseCaseManipulationInteractor<Product>, IProductUseCasesReadOnlyInteractor
    {
        Task<UseCaseResult<RentResult>> RentProduct(RentRequirement rentProductRequirement);
        Task<UseCaseResult<RentFinishedResult>> FinishRent(FinishRentRequirement rentProductRequirement);

    }

    public class ProductUseCasesManipulationInteractor : BaseUseCaseManipulationInteractor<Product, IProductUseCasesReadOnlyInteractor>, IProductUseCasesManipulationInteractor
    {
        private readonly IRentProduct _rentProduct;
        private readonly IFinishRent _finishRent;

        public ProductUseCasesManipulationInteractor(IManipulationPersistenceGateway<Product> basePersistenceGateway, IProductUseCasesReadOnlyInteractor subjectUseCasesReadOnlyInteractor, IRentProduct rentProduct, IFinishRent finishRent) : base(basePersistenceGateway, subjectUseCasesReadOnlyInteractor)
        {
            _rentProduct = rentProduct;
            _finishRent = finishRent;
        }

        public async Task<UseCaseResult<RentResult>> RentProduct(RentRequirement rentProductRequirement)
        {
            var rentProductResponse = await _rentProduct.Execute(rentProductRequirement);
            
            return rentProductResponse.Success
                ? UseCasesResponses.SuccessfullyExecutedResponse(rentProductResponse.Result, rentProductResponse.Message)
                : UseCasesResponses.ExecutionFailureResponse<RentResult>(rentProductResponse.Message);
        }

        public async Task<UseCaseResult<RentFinishedResult>> FinishRent(FinishRentRequirement rentProductRequirement)
        {
            var finishRentResponse = await _finishRent.Execute(rentProductRequirement);

            return finishRentResponse.Success
                ? UseCasesResponses.SuccessfullyExecutedResponse(finishRentResponse.Result)
                : UseCasesResponses.ExecutionFailureResponse<RentFinishedResult>(finishRentResponse.Message);
        }


        public async Task<UseCaseResult<decimal>> GetProductRentPrice(GetProductRentPriceRequirement rentPriceRequirement)
        {
            return await UseCasesReadOnlyInteractor.GetProductRentPrice(rentPriceRequirement);
        }
    }
}