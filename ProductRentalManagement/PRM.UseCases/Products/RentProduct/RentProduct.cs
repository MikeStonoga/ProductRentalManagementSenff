using System.Threading.Tasks;
using PRM.Domain.Products;
using PRM.Domain.Products.Rents.Dtos;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;

namespace PRM.UseCases.Products.RentProduct
{
    public interface IRentProduct : IBaseUseCase<RentRequirement, RentResult> {
    
    }
    
    public class RentProduct : BaseUseCase<RentRequirement, RentResult>, IRentProduct
    {
        private readonly IManipulationPersistenceGateway<Product> _products;

        public RentProduct(IManipulationPersistenceGateway<Product> products)
        {
            _products = products;
        }

        public override async Task<UseCaseResult<RentResult>> Execute(RentRequirement requirement)
        {
            var productToRent = await _products.GetById(requirement.ProductId);
            if (!productToRent.Success) return UseCasesResponses.PersistenceErrorResponse<RentResult>(productToRent.Message);
            if (!productToRent.Response.IsAvailable) return UseCasesResponses.ValidationErrorResponse<RentResult>("ProductNotAvailableToRent");

            var rentProductResponse = productToRent.Response.RentProduct(requirement);
            if (!rentProductResponse.Success) return UseCasesResponses.ValidationErrorResponse<RentResult>(rentProductResponse.Message);

            var updateProductResponse = await _products.Update(productToRent.Response);
            return !updateProductResponse.Success 
                ? UseCasesResponses.PersistenceErrorResponse<RentResult>(updateProductResponse.Message)
                : UseCasesResponses.UseCaseSuccessfullyExecutedResponse(rentProductResponse.Result, rentProductResponse.Message);
            
        }
    }
}