using System.Threading.Tasks;
using PRM.Domain.Products;
using PRM.Domain.Rents;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;

namespace PRM.UseCases.Rents.FinishRents
{
    public interface IFinishRent : IBaseUseCase<FinishRentRequirement, FinishRentResult>
    {
        
    }
    
    public class FinishRent : BaseUseCase<FinishRentRequirement, FinishRentResult>, IFinishRent
    {
        private readonly IManipulationPersistenceGateway<Rent> _rents;
        private readonly IManipulationPersistenceGateway<Product> _products; 

        public FinishRent(IManipulationPersistenceGateway<Rent> rents, IManipulationPersistenceGateway<Product> products)
        {
            _rents = rents;
            _products = products;
        }

        public override async Task<UseCaseResult<FinishRentResult>> Execute(FinishRentRequirement requirement)
        {
            var rentToFinish = await _rents.GetById(requirement.RentId);
            if (!rentToFinish.Success) return UseCasesResponses.ExecutionFailureResponse<FinishRentResult>(rentToFinish.Message);

            var finishRentResponse = rentToFinish.Response.FinishRent(requirement.DamageFee, requirement.Discount);
            if (!finishRentResponse.Success) return UseCasesResponses.ExecutionFailureResponse<FinishRentResult>(finishRentResponse.Message);

            foreach (var product in finishRentResponse.Result.Products)
            {
                product.MarkAsAvailable();
                await _products.Update(product);
            }

            await _rents.Update(finishRentResponse.Result);
            var finishRentResult = new FinishRentResult(finishRentResponse.Result.CurrentRentPaymentValue);
            return UseCasesResponses.SuccessfullyExecutedResponse(finishRentResult);
        }
    }
}