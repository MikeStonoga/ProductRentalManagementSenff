using PRM.Domain.Products;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;

namespace PRM.UseCases.Products
{
    public interface IProductUseCasesReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<Product>
    {
    }
        
    public class ProductUseCasesReadOnlyInteractor : BaseUseCaseReadOnlyInteractor<Product>, IProductUseCasesReadOnlyInteractor
    {
        
        public ProductUseCasesReadOnlyInteractor(IReadOnlyPersistenceGateway<Product> baseReadOnlyPersistenceGateway) : base(baseReadOnlyPersistenceGateway)
        {
        }
        
    }

    public interface IProductUseCasesManipulationInteractor : IBaseUseCaseManipulationInteractor<Product>, IProductUseCasesReadOnlyInteractor
    {

    }

    public class ProductUseCasesManipulationInteractor : BaseUseCaseManipulationInteractor<Product, IProductUseCasesReadOnlyInteractor>, IProductUseCasesManipulationInteractor
    {
        public ProductUseCasesManipulationInteractor(IManipulationPersistenceGateway<Product> basePersistenceGateway, IProductUseCasesReadOnlyInteractor subjectUseCasesReadOnlyInteractor, IManipulationPersistenceGateway<ProductRentalHistory> productRentalHistories) : base(basePersistenceGateway, subjectUseCasesReadOnlyInteractor)
        {
        }
    }
}