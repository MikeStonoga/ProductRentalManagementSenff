using System.Threading.Tasks;
using PRM.Domain.Rents;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;
using PRM.UseCases.Rents.RentProducts;

namespace PRM.UseCases.Rents
{
    public interface IRentUseCasesReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<Rent>
    {
    }
        
    public class RentUseCasesReadOnlyInteractor : BaseUseCaseReadOnlyInteractor<Rent>, IRentUseCasesReadOnlyInteractor
    {
        
        public RentUseCasesReadOnlyInteractor(IReadOnlyPersistenceGateway<Rent> baseReadOnlyPersistenceGateway) : base(baseReadOnlyPersistenceGateway)
        {
        }
        
    }

    public interface IRentUseCasesManipulationInteractor : IBaseUseCaseManipulationInteractor<Rent>, IRentUseCasesReadOnlyInteractor
    {
        Task<UseCaseResult<Rent>> RentProduct(RentProductRequeriment requeriment);
    }

    public class RentUseCasesManipulationInteractor : BaseUseCaseManipulationInteractor<Rent, IRentUseCasesReadOnlyInteractor>, IRentUseCasesManipulationInteractor
    {

        private readonly IRentProduct _rentProduct;

        public RentUseCasesManipulationInteractor(IManipulationPersistenceGateway<Rent> basePersistenceGateway, IRentUseCasesReadOnlyInteractor subjectUseCasesReadOnlyInteractor, IRentProduct rentProduct) : base(basePersistenceGateway, subjectUseCasesReadOnlyInteractor)
        {
            _rentProduct = rentProduct;
        }


        public async Task<UseCaseResult<Rent>> RentProduct(RentProductRequeriment requeriment)
        {
            var executionResult = await _rentProduct.Execute(requeriment);
            return !executionResult.Success 
                ? UseCasesResponses.ExecutionFailureResponse<Rent>(executionResult.Message) 
                : UseCasesResponses.SuccessfullyExecutedResponse(executionResult.Result);
        }
    }
}