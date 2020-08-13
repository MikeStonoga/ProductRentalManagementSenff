using System.Linq;
using System.Threading.Tasks;
using PRM.Domain.Products;
using PRM.Domain.Products.Rents.Dtos;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;

namespace PRM.UseCases.Products.FinishRent
{
    public interface IFinishRent : IBaseUseCase<FinishRentRequirement, RentFinishedResult>
    {
        
    }
    
    public class FinishRent : BaseUseCase<FinishRentRequirement, RentFinishedResult>, IFinishRent
    {
        private readonly IManipulationPersistenceGateway<Product> _products;

        public FinishRent(IManipulationPersistenceGateway<Product> products)
        {
            _products = products;
        }

        public override async Task<UseCaseResult<RentFinishedResult>> Execute(FinishRentRequirement requirement)
        {
            var productWithRentToFinish = await _products.First(product => product.Rents.Select(rent => rent.Id).Contains(requirement.RentId), product => product.Rents);
            
            if (!productWithRentToFinish.Success) return UseCasesResponses.PersistenceErrorResponse<RentFinishedResult>(productWithRentToFinish.Message);

            var rentFinishedResult = productWithRentToFinish.Response.FinishProductRent(requirement);
            if (!rentFinishedResult.Success) return UseCasesResponses.ValidationErrorResponse<RentFinishedResult>(rentFinishedResult.Message);

            var productUpdateResponse = await _products.Update(productWithRentToFinish.Response);
            
            return !rentFinishedResult.Success 
                ? UseCasesResponses.PersistenceErrorResponse<RentFinishedResult>(productUpdateResponse.Message) 
                : UseCasesResponses.SuccessfullyExecutedResponse(rentFinishedResult.Result);
        }
    }
}