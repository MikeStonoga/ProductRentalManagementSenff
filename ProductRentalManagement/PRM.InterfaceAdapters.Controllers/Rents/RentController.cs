using System;
using System.Threading.Tasks;
using PRM.Domain.BaseCore.ValueObjects;
using PRM.Domain.Rents;
using PRM.InterfaceAdapters.Controllers.BaseCore;
using PRM.InterfaceAdapters.Controllers.BaseCore.Extensions;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos.FinishRents;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos.GetOpenRentsPaymentForecasts;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos.GetRentForecastPrices;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos.RentProducts;
using PRM.UseCases.Rents;
using PRM.UseCases.Rents.FinishRents;
using PRM.UseCases.Rents.GetOpenRentsPaymentForecasts;
using PRM.UseCases.Rents.GetRentForecastPrices;
using PRM.UseCases.Rents.RentProducts;

namespace PRM.InterfaceAdapters.Controllers.Rents
{
    public interface IRentReadOnlyController : IBaseReadOnlyController<Rent, RentOutput>
    {
        Task<ApiResponse<GetRentForecastPriceOutput>> GetRentForecastPrice(GetRentForecastPriceInput input);
        Task<ApiResponse<GetAllResponse<Rent, RentOutput>>> GetOpenRents();
        Task<ApiResponse<GetAllResponse<Rent, RentOutput>>> GetOpenRentsFromPeriod(DateTime? startDate, DateTime? endDate);
        Task<ApiResponse<GetAllResponse<Rent, RentOutput>>> GetLateRents();
        Task<ApiResponse<GetAllResponse<Rent, RentOutput>>> GetClosedRentsFromPeriod(DateTime? startDate, DateTime? endDate);
        Task<ApiResponse<GetOpenRentsPaymentForecastOutput>> GetOpenRentsPaymentForecast(GetOpenRentsPaymentForecastInput input);
    }
     
    public class RentReadOnlyController : BaseReadOnlyController<Rent, RentOutput, IRentUseCasesReadOnlyInteractor>, IRentReadOnlyController
    {
        private readonly IRentUseCasesReadOnlyInteractor _useCasesReadOnlyInteractor;

        public RentReadOnlyController(IRentUseCasesReadOnlyInteractor useCasesReadOnlyInteractor) : base(useCasesReadOnlyInteractor)
        {
            _useCasesReadOnlyInteractor = useCasesReadOnlyInteractor;
        }

        public async Task<ApiResponse<GetRentForecastPriceOutput>> GetRentForecastPrice(GetRentForecastPriceInput input)
        {
            return await ApiResponses.GetUseCaseInteractorResponse<GetRentForecastPriceRequirement, GetRentForecastPriceResult, GetRentForecastPriceInput, GetRentForecastPriceOutput>(_useCasesReadOnlyInteractor.GetRentForecastPrice, input);
        }

        public async Task<ApiResponse<GetAllResponse<Rent, RentOutput>>> GetOpenRents()
        {
            return await GetAll(r => r.IsFinished == false);
        }

        public async Task<ApiResponse<GetAllResponse<Rent, RentOutput>>> GetOpenRentsFromPeriod(DateTime? startDate, DateTime? endDate)
        {
            var period = DateRangeProvider.GetDateRange(startDate ??= DateTime.MinValue, endDate ??= DateTime.MaxValue);
            if (!period.Success) return ApiResponses.FailureResponse<GetAllResponse<Rent, RentOutput>>(period.Message);
            
            return await GetAll(r => r.IsFinished == false && period.Result.IsOnRange(r.RentPeriod));
        }

        public async Task<ApiResponse<GetAllResponse<Rent, RentOutput>>> GetLateRents()
        {
            return await GetAll(r => r.IsLate);
        }

        public async Task<ApiResponse<GetAllResponse<Rent, RentOutput>>> GetClosedRentsFromPeriod(DateTime? startDate, DateTime? endDate)
        {
            var period = DateRangeProvider.GetDateRange(startDate ??= DateTime.MinValue, endDate ??= DateTime.MaxValue);
            if (!period.Success) return ApiResponses.FailureResponse<GetAllResponse<Rent, RentOutput>>(period.Message);
            
            return await GetAll(r => r.IsFinished && period.Result.IsOnRange(r.RentPeriod));
        }

        public async Task<ApiResponse<GetOpenRentsPaymentForecastOutput>> GetOpenRentsPaymentForecast(GetOpenRentsPaymentForecastInput input)
        {
            return await ApiResponses.GetUseCaseInteractorResponse<GetOpenRentsPaymentForecastRequirement, GetOpenRentsPaymentForecastResult, GetOpenRentsPaymentForecastInput, GetOpenRentsPaymentForecastOutput>(_useCasesReadOnlyInteractor.GetOpenRentsPaymentForecast, input);
        }
    }

    public interface IRentManipulationController : IRentReadOnlyController
    {
        Task<ApiResponse<RentProductsOutput>> RentProducts(RentProductsInput input);
        Task<ApiResponse<FinishRentOutput>> FinishRent(FinishRentInput input);
    }

    public class RentManipulationController : RentReadOnlyController, IRentManipulationController
    {
        private readonly IRentUseCasesManipulationInteractor _useCasesInteractor;

        public RentManipulationController(IRentUseCasesManipulationInteractor useCasesInteractor) : base(useCasesInteractor)
        {
            _useCasesInteractor = useCasesInteractor;
        }

        public async Task<ApiResponse<RentProductsOutput>> RentProducts(RentProductsInput input)
        {
            return await ApiResponses.GetUseCaseInteractorResponse<RentProductsRequirement, RentProductsResult, RentProductsInput, RentProductsOutput>(_useCasesInteractor.RentProducts, input);
        }

        public async Task<ApiResponse<FinishRentOutput>> FinishRent(FinishRentInput input)
        {
            return await ApiResponses.GetUseCaseInteractorResponse<FinishRentRequirement, FinishRentResult, FinishRentInput, FinishRentOutput>(_useCasesInteractor.FinishRent, input);
        }
    }
}