using Microsoft.AspNetCore.Mvc;
using PRM.Domain.Rents;
using PRM.Infrastructure.ApplicationDelivery.WebApiHost.BaseCore;
using PRM.InterfaceAdapters.Controllers.Rents;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos;
using PRM.UseCases.Rents;

namespace PRM.Infrastructure.ApplicationDelivery.WebApiHost.Rents
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class RentController : BaseManipulationWebController<Rent, RentInput, RentOutput, IRentUseCasesManipulationInteractor, IRentManipulationController>, IRentManipulationController
    {
        public RentController(IRentUseCasesManipulationInteractor useCaseInteractor, IRentManipulationController manipulationController) : base(useCaseInteractor, manipulationController)
        {
        }
    }
}