using PRM.Domain.Renters;
using PRM.InterfaceAdapters.Controllers.BaseCore;
using PRM.InterfaceAdapters.Controllers.Renters.Dtos;
using PRM.UseCases.Renters;

namespace PRM.InterfaceAdapters.Controllers.Renters
{
    public interface IRenterReadOnlyController : IBaseReadOnlyController<Renter, RenterOutput>
    {
    }
     
    public class RenterReadOnlyController : BaseReadOnlyController<Renter, RenterOutput, IRenterUseCasesReadOnlyInteractor>, IRenterReadOnlyController
    {
        public RenterReadOnlyController(IRenterUseCasesReadOnlyInteractor useCaseReadOnlyInteractor) : base(useCaseReadOnlyInteractor)
        {
        }
    }

    public interface IRenterManipulationController : IBaseManipulationController<Renter, RenterInput, RenterOutput>, IRenterReadOnlyController
    {

    }

    public class RenterManipulationController : BaseManipulationController<Renter, RenterInput, RenterOutput, IRenterUseCasesManipulationInteractor, IRenterReadOnlyController>, IRenterManipulationController
    {
        public RenterManipulationController(IRenterUseCasesManipulationInteractor useCaseInteractor, IRenterReadOnlyController readOnlyController) : base(useCaseInteractor, readOnlyController)
        {
        }
    }
}