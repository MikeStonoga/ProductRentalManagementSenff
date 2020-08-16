using System;
using System.Threading.Tasks;
using PRM.Domain.BaseCore.ValueObjects;
using PRM.Domain.Products;
using PRM.Domain.Renters;
using PRM.Domain.Rents;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;
using PRM.UseCases.Rents.Validations.Requirements.Rents;

namespace PRM.UseCases.Rents.GetRentForecastPrices
{
    public interface IGetRentForecastPrice : IBaseUseCase<GetRentForecastPriceRequirement, GetRentForecastPriceResult>
    {
    }
    
    public class GetRentForecastPrice : BaseUseCase<GetRentForecastPriceRequirement, GetRentForecastPriceResult>, IGetRentForecastPrice
    {
        private readonly IValidateRentRequirement _validateRentRequirement;

        public GetRentForecastPrice(IValidateRentRequirement validateRentRequirement)
        {
            _validateRentRequirement = validateRentRequirement;
        }

        public override async Task<UseCaseResult<GetRentForecastPriceResult>> Execute(GetRentForecastPriceRequirement getRentForecastPriceRequirement)
        {
            var validationResponse = await _validateRentRequirement.Validate(getRentForecastPriceRequirement);
            if (!validationResponse.Success) return UseCasesResponses.ExecutionFailure<GetRentForecastPriceResult>(validationResponse.Message);
            
            var rentForecastPrice = new Rent(validationResponse.Result.RentPeriod, validationResponse.Result.Products, validationResponse.Result.Renter).GetRentForecastPrice();
            
            var rentForecastPriceResult =new GetRentForecastPriceResult(rentForecastPrice);
            return UseCasesResponses.SuccessfullyExecuted(rentForecastPriceResult);
        }
    }
}