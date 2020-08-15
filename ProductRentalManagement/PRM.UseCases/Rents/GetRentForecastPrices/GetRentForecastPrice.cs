using System.Threading.Tasks;
using PRM.Domain.Products;
using PRM.Domain.Rents;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;

namespace PRM.UseCases.Rents.GetRentForecastPrices
{
    public interface IGetRentForecastPrice : IBaseUseCase<GetRentForecastPriceRequirement, GetRentForecastPriceResult>
    {
    }
    
    public class GetRentForecastPrice : BaseUseCase<GetRentForecastPriceRequirement, GetRentForecastPriceResult>, IGetRentForecastPrice
    {
        private readonly IReadOnlyPersistenceGateway<Product> _products;

        public GetRentForecastPrice(IReadOnlyPersistenceGateway<Product> products)
        {
            _products = products;
        }

        public override async Task<UseCaseResult<GetRentForecastPriceResult>> Execute(GetRentForecastPriceRequirement rentProductsRequirement)
        {
            var productsToRentResponse = await _products.GetByIds(rentProductsRequirement.ProductsIds);
            if (!productsToRentResponse.Success) return UseCasesResponses.ExecutionFailureResponse<GetRentForecastPriceResult>(productsToRentResponse.Message);
            
            var rentForecastPrice = new Rent(rentProductsRequirement.RentRequirement, productsToRentResponse.Response).GetRentForecastPrice();
            var rentForecastPriceResult =new GetRentForecastPriceResult(rentForecastPrice);
            return UseCasesResponses.SuccessfullyExecutedResponse(rentForecastPriceResult);
        }
    }
}