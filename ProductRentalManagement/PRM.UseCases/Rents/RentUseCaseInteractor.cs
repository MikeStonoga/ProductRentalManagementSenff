using System.Threading.Tasks;
using PRM.Domain.Rents;
using PRM.Domain.Rents.Dtos;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;
using PRM.UseCases.Rents.FinishRents;
using PRM.UseCases.Rents.GetRentForecastPrices;
using PRM.UseCases.Rents.RentProducts;

namespace PRM.UseCases.Rents
{
    public interface IRentUseCasesReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<Rent>
    {
        Task<UseCaseResult<GetRentForecastPriceResult>> GetRentForecastPrice(GetRentForecastPriceRequirement requirement);
    }
        
    public class RentUseCasesReadOnlyInteractor : BaseUseCaseReadOnlyInteractor<Rent>, IRentUseCasesReadOnlyInteractor
    {
        private readonly IGetRentForecastPrice _getRentForecastPrice;
        protected readonly IReadOnlyPersistenceGateway<Rent> readOnlyPersistenceGateway;
        public RentUseCasesReadOnlyInteractor(IReadOnlyPersistenceGateway<Rent> readOnlyPersistenceGateway, IGetRentForecastPrice getRentForecastPrice) : base(readOnlyPersistenceGateway)
        {
            this.readOnlyPersistenceGateway = readOnlyPersistenceGateway;
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

        public RentUseCasesManipulationInteractor(IManipulationPersistenceGateway<Rent> manipulationPersistenceGateway, IGetRentForecastPrice getRentForecastPrice, IRentProducts rentProducts, IFinishRent finishRent) : base(manipulationPersistenceGateway, getRentForecastPrice)
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