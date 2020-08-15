using PRM.Domain.Rents;
using PRM.InterfaceAdapters.Controllers.BaseCore;
using PRM.InterfaceAdapters.Controllers.Rents.Dtos;
using PRM.UseCases.Rents;

namespace PRM.InterfaceAdapters.Controllers.Rents
{
    public interface IRentReadOnlyController : IBaseReadOnlyController<Rent, RentOutput>
    {
    }
     
    public class RentReadOnlyController : BaseReadOnlyController<Rent, RentOutput, IRentUseCasesReadOnlyInteractor>, IRentReadOnlyController
    {
        public RentReadOnlyController(IRentUseCasesReadOnlyInteractor useCaseReadOnlyInteractor) : base(useCaseReadOnlyInteractor)
        {
        }
    }

    public interface IRentManipulationController : IBaseManipulationController<Rent, RentInput, RentOutput>, IRentReadOnlyController
    {

    }

    public class RentManipulationController : BaseManipulationController<Rent, RentInput, RentOutput, IRentUseCasesManipulationInteractor, IRentReadOnlyController>, IRentManipulationController
    {
        public RentManipulationController(IRentUseCasesManipulationInteractor useCaseInteractor, IRentReadOnlyController readOnlyController) : base(useCaseInteractor, readOnlyController)
        {
        }
    }
}