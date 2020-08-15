using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRM.Domain.Rents;
using PRM.Infrastructure.ApplicationDelivery.WebApiHost.BaseCore;
using PRM.InterfaceAdapters.Controllers.BaseCore;
using PRM.InterfaceAdapters.Controllers.Rents;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos.FinishRents;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos.RentProducts;
using PRM.UseCases.Rents;

namespace PRM.Infrastructure.ApplicationDelivery.WebApiHost.Rents
{
    public class RentController : BaseReadOnlyWebController<Rent, RentOutput, IRentReadOnlyController, IRentUseCasesReadOnlyInteractor>,  IRentManipulationController
    {
        private readonly IRentManipulationController _rentManipulationController;
        public RentController(IRentUseCasesManipulationInteractor useCasesInteractor, IRentReadOnlyController readOnlyController, IRentManipulationController rentManipulationController) : base(useCasesInteractor, readOnlyController)
        {
            _rentManipulationController = rentManipulationController;
        }

        public async Task<ApiResponse<RentProductsOutput>> RentProducts(RentProductsInput input)
        {
            return await _rentManipulationController.RentProducts(input);
        }

        public async Task<ApiResponse<FinishRentOutput>> FinishRent(FinishRentInput input)
        {
            return await _rentManipulationController.FinishRent(input);
        }
    }
}