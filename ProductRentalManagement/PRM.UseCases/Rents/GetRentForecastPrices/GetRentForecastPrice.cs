using System.Threading.Tasks;
using PRM.UseCases.BaseCore;

namespace PRM.UseCases.Rents.GetRentForecastPrices
{
    public interface IGetRentForecastPrice : IBaseUseCase<GetRentForecastPriceRequirement, GetRentForecastPriceResult>
    {
    }
    
    public class GetRentForecastPrice : BaseUseCase<GetRentForecastPriceRequirement, GetRentForecastPriceResult>, IGetRentForecastPrice
    {
        public override async Task<UseCaseResult<GetRentForecastPriceResult>> Execute(GetRentForecastPriceRequirement rentProductsRequirement)
        {
            throw new System.NotImplementedException();
        }
    }

    public class GetRentForecastPriceResult
    {
    }

    public class GetRentForecastPriceRequirement
    {
    }
}