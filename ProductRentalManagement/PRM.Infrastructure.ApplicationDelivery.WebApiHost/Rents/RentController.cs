using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PRM.Domain.Rents;
using PRM.Infrastructure.ApplicationDelivery.WebApiHost.BaseCore;
using PRM.InterfaceAdapters.Controllers.BaseCore;
using PRM.InterfaceAdapters.Controllers.Rents;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos.FinishRents;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos.GetOpenRentsPaymentForecasts;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos.GetRentForecastPrices;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos.RentProducts;
using PRM.UseCases.Rents;

namespace PRM.Infrastructure.ApplicationDelivery.WebApiHost.Rents
{
    public interface IRentController : IBaseReadOnlyWebController<Rent, RentOutput>, IRentManipulationController
    {
    }
    public class RentController : BaseReadOnlyWebController<Rent, RentOutput, IRentReadOnlyController, IRentUseCasesReadOnlyInteractor>, IRentController
    {
        private readonly IRentManipulationController _rentManipulationController;
        public RentController(IRentUseCasesManipulationInteractor useCasesInteractor, IRentReadOnlyController readOnlyController, IRentManipulationController rentManipulationController) : base(useCasesInteractor, readOnlyController)
        {
            _rentManipulationController = rentManipulationController;
        }

        [HttpPost]
        public async Task<ApiResponse<RentProductsOutput>> RentProducts([FromBody] RentProductsInput input)
        {
            return await _rentManipulationController.RentProducts(input);
        }

        [HttpPut]
        public async Task<ApiResponse<FinishRentOutput>> FinishRent([FromBody] FinishRentInput input)
        {
            return await _rentManipulationController.FinishRent(input);
        }

        [HttpPost]
        public async Task<ApiResponse<GetRentForecastPriceOutput>> GetRentForecastPrice([FromBody] GetRentForecastPriceInput input)
        {
            return await _rentManipulationController.GetRentForecastPrice(input);
        }

        [HttpGet]
        public async Task<ApiResponse<GetAllResponse<Rent, RentOutput>>> GetOpenRents()
        {
            return await _rentManipulationController.GetOpenRents();
        }

        public async Task<ApiResponse<GetAllResponse<Rent, RentOutput>>> GetOpenRentsFromPeriod(DateTime? startDate, DateTime? endDate)
        {
            return await _rentManipulationController.GetOpenRentsFromPeriod(startDate, endDate);
        }

        [HttpGet]
        public async Task<ApiResponse<GetAllResponse<Rent, RentOutput>>> GetLateRents()
        {
            return await _rentManipulationController.GetLateRents();
        }

        public async Task<ApiResponse<GetAllResponse<Rent, RentOutput>>> GetClosedRentsFromPeriod(DateTime? startDate, DateTime? endDate)
        {
            return await _rentManipulationController.GetClosedRentsFromPeriod(startDate, endDate);
        }

        [HttpPost]
        public async Task<ApiResponse<GetOpenRentsPaymentForecastOutput>> GetOpenRentsPaymentForecast(GetOpenRentsPaymentForecastInput input)
        {
            input.TargetDate ??= DateTime.Now;
            return await _rentManipulationController.GetOpenRentsPaymentForecast(input);
        }
    }
}