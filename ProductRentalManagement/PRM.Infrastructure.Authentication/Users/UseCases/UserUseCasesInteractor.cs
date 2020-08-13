using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;

namespace PRM.Infrastructure.Authentication.Users.UseCases
{
    public interface IUserUseCasesReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<User>
    {
    }
        
    public class UserUseCasesReadOnlyInteractor : BaseUseCaseReadOnlyInteractor<User>, IUserUseCasesReadOnlyInteractor
    {
        
        public UserUseCasesReadOnlyInteractor(IReadOnlyPersistenceGateway<User> baseReadOnlyPersistenceGateway) : base(baseReadOnlyPersistenceGateway)
        {
        }

    }

    public interface IUserUseCasesManipulationInteractor : IBaseUseCaseManipulationInteractor<User>, IUserUseCasesReadOnlyInteractor
    {
    }

    public class UserUseCasesManipulationInteractor : BaseUseCaseManipulationInteractor<User, IUserUseCasesReadOnlyInteractor>, IUserUseCasesManipulationInteractor
    {

        public UserUseCasesManipulationInteractor(IManipulationPersistenceGateway<User> basePersistenceGateway, IUserUseCasesReadOnlyInteractor subjectUseCasesReadOnlyInteractor) : base(basePersistenceGateway, subjectUseCasesReadOnlyInteractor)
        {

        }


    }
}