using PRM.Domain.Renters;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;

namespace PRM.UseCases.Renters
{
    public interface IRenterUseCasesReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<Renter>
    {
    }
        
    public class RenterUseCasesReadOnlyInteractor : BaseUseCaseReadOnlyInteractor<Renter>, IRenterUseCasesReadOnlyInteractor
    {
        
        public RenterUseCasesReadOnlyInteractor(IReadOnlyPersistenceGateway<Renter> baseReadOnlyPersistenceGateway) : base(baseReadOnlyPersistenceGateway)
        {
        }
        
    }

    public interface IRenterUseCasesManipulationInteractor : IBaseUseCaseManipulationInteractor<Renter>, IRenterUseCasesReadOnlyInteractor
    {

    }

    public class RenterUseCasesManipulationInteractor : BaseUseCaseManipulationInteractor<Renter, IRenterUseCasesReadOnlyInteractor>, IRenterUseCasesManipulationInteractor
    {
        public RenterUseCasesManipulationInteractor(IManipulationPersistenceGateway<Renter> basePersistenceGateway, IRenterUseCasesReadOnlyInteractor subjectUseCasesReadOnlyInteractor, IManipulationPersistenceGateway<RenterRentalHistory> productRentalHistories) : base(basePersistenceGateway, subjectUseCasesReadOnlyInteractor)
        {
        }
    }
}