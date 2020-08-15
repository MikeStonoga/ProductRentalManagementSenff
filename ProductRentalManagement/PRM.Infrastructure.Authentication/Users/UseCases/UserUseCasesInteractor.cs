using System.Threading.Tasks;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;

namespace PRM.Infrastructure.Authentication.Users.UseCases
{
    public interface IUserUseCasesReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<User>
    {
    }
        
    public class UserUseCasesReadOnlyInteractor : BaseUseCaseReadOnlyInteractor<User>, IUserUseCasesReadOnlyInteractor
    {
        
        public UserUseCasesReadOnlyInteractor(IReadOnlyPersistenceGateway<User> readOnlyPersistenceGateway) : base(readOnlyPersistenceGateway)
        {
        }

    }

    public interface IUserUseCasesManipulationInteractor : IBaseUseCaseManipulationInteractor<User>, IUserUseCasesReadOnlyInteractor
    {
    }

    public class UserUseCasesManipulationInteractor : BaseUseCaseManipulationInteractor<User, IUserUseCasesReadOnlyInteractor>, IUserUseCasesManipulationInteractor
    {
        private readonly IManipulationPersistenceGateway<User> _persistenceGateway;
        public UserUseCasesManipulationInteractor(IManipulationPersistenceGateway<User> persistenceGateway, IUserUseCasesReadOnlyInteractor useCasesReadOnlyInteractor) : base(persistenceGateway, useCasesReadOnlyInteractor)
        {
            _persistenceGateway = persistenceGateway;
        }
        
        public override async Task<UseCaseResult<User>> Create(User entity)
        {
            var loginValidationResponse = await _persistenceGateway.First(user => user.Login == entity.Login);
            if (loginValidationResponse.Success)
            {
                loginValidationResponse.Success = false;
                loginValidationResponse.Message = "AlreadyHasLogin";
                return GetUseCaseResult(loginValidationResponse);
            }
            
            var persistenceResponse = await _persistenceGateway.Create(entity);
            return GetUseCaseResult(persistenceResponse);
        }
    }
}