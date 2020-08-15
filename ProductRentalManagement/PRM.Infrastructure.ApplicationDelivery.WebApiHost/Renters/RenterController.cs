using PRM.Domain.Renters;
using PRM.Infrastructure.ApplicationDelivery.WebApiHost.BaseCore;
using PRM.InterfaceAdapters.Controllers.Renters;
using PRM.InterfaceAdapters.Controllers.Renters.Dtos;
using PRM.UseCases.Renters;

namespace PRM.Infrastructure.ApplicationDelivery.WebApiHost.Renters
{
    public class RenterController : BaseManipulationWebController<Renter, RenterInput, RenterOutput, IRenterUseCasesManipulationInteractor, IRenterManipulationController>, IRenterManipulationController
    {
        public RenterController(IRenterUseCasesManipulationInteractor useCaseInteractor, IRenterManipulationController manipulationController) : base(useCaseInteractor, manipulationController)
        {
        }
    }
}