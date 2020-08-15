using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRM.InterfaceAdapters.Controllers.Rents;
using PRM.UseCases.Rents;

namespace PRM.Infrastructure.ApplicationDelivery.WebApiHost.Rents
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class RentController : RentManipulationController
    {
        public RentController(IRentUseCasesManipulationInteractor useCasesInteractor) : base(useCasesInteractor)
        {
        }
    }
}