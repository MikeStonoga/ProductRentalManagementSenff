using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PRM.Domain.Rents;
using PRM.Infrastructure.ApplicationDelivery.WebApiHost.BaseCore;
using PRM.InterfaceAdapters.Controllers.BaseCore;
using PRM.InterfaceAdapters.Controllers.Rents;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos.FinishRents;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos.GetRentForecastPrices;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos.RentProducts;
using PRM.UseCases.Rents;

namespace PRM.Infrastructure.ApplicationDelivery.WebApiHost.Rents
{
    public class RentController : BaseReadOnlyWebController<Rent, RentOutput, IRentReadOnlyController, IRentUseCasesReadOnlyInteractor>, IRentManipulationController
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
    }
}