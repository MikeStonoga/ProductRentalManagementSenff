using System.Threading.Tasks;
using PRM.Domain.Rents.Dtos;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;
using PRM.UseCases.Rents.FinishRents;
using PRM.UseCases.Rents.GetRentForecastPrices;
using PRM.UseCases.Rents.RentProducts;

namespace PRM.UseCases.Rents
{
    public interface IRentUseCasesReadOnlyInteractor
    {
        Task<UseCaseResult<GetRentForecastPriceResult>> GetRentForecastPrice(GetRentForecastPriceRequirement requirement);
    }
        
    public class RentUseCasesReadOnlyInteractor : IRentUseCasesReadOnlyInteractor
    {
        private readonly IGetRentForecastPrice _getRentForecastPrice;

        public RentUseCasesReadOnlyInteractor(IGetRentForecastPrice getRentForecastPrice)
        {
            _getRentForecastPrice = getRentForecastPrice;
        }

        public async Task<UseCaseResult<GetRentForecastPriceResult>> GetRentForecastPrice(GetRentForecastPriceRequirement requirement)
        {
            return await UseCasesResponses.GetUseCaseExecutionResponse<IGetRentForecastPrice, GetRentForecastPriceRequirement, GetRentForecastPriceResult>(_getRentForecastPrice, requirement);
        }
    }

    public interface IRentUseCasesManipulationInteractor : IRentUseCasesReadOnlyInteractor
    {
        Task<UseCaseResult<FinishRentResult>> FinishRent(FinishRentRequirement requirement);
        Task<UseCaseResult<RentProductsResult>> RentProducts(RentProductsRequirement requirement);
    }

    public class RentUseCasesManipulationInteractor : RentUseCasesReadOnlyInteractor, IRentUseCasesManipulationInteractor
    {
        
        private readonly IRentProducts _rentProducts;
        private readonly IFinishRent _finishRent;

        public RentUseCasesManipulationInteractor(IGetRentForecastPrice getRentForecastPrice, IRentProducts rentProducts, IFinishRent finishRent) : base(getRentForecastPrice)
        {
            _rentProducts = rentProducts;
            _finishRent = finishRent;
        }


        public async Task<UseCaseResult<FinishRentResult>> FinishRent(FinishRentRequirement requirement)
        {
            return await UseCasesResponses.GetUseCaseExecutionResponse<IFinishRent, FinishRentRequirement, FinishRentResult>(_finishRent, requirement);
        }

        public async Task<UseCaseResult<RentProductsResult>> RentProducts(RentProductsRequirement requirement)
        {
            return await UseCasesResponses.GetUseCaseExecutionResponse<IRentProducts, RentProductsRequirement, RentProductsResult>(_rentProducts, requirement);
        }
        
        
    }
}