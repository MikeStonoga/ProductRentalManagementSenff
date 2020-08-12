using PRM.Domain.Products;
using PRM.InterfaceAdapters.Controllers.BaseCore;
using PRM.InterfaceAdapters.Controllers.Products.Dtos;
using PRM.UseCases.Products;

namespace PRM.InterfaceAdapters.Controllers.Products
{
    public interface IProductReadOnlyController : IBaseReadOnlyController<Product, ProductOutput>
    {
        
    }
     
    public abstract class ProductReadOnlyController : BaseReadOnlyController<Product, ProductOutput, IProductUseCasesReadOnlyInteractor>
    {
        protected ProductReadOnlyController(IProductUseCasesReadOnlyInteractor baseConsoleUseCaseManipulationReadOnlyInteractor) : base(baseConsoleUseCaseManipulationReadOnlyInteractor)
        {
        }
    }

    public interface IProductManipulationController : IBaseManipulationController<Product, ProductInput, ProductOutput>, IProductReadOnlyController
    {
        
    }

    public abstract class ProductManipulationController : BaseManipulationController<Product, ProductInput, ProductOutput, IProductUseCasesManipulationInteractor, IProductReadOnlyController>, IProductManipulationController
    {
        protected ProductManipulationController(IProductUseCasesManipulationInteractor baseConsoleUseCaseManipulationInteractor, IProductReadOnlyController readOnlyController) : base(baseConsoleUseCaseManipulationInteractor, readOnlyController)
        {
        }
    }
}