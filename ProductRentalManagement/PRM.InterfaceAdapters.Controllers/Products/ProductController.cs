using PRM.Domain.Products;
using PRM.InterfaceAdapters.Controllers.BaseCore;
using PRM.InterfaceAdapters.Controllers.Products.Dtos;
using PRM.UseCases.Products;

namespace PRM.InterfaceAdapters.Controllers.Products
{
    public interface IProductReadOnlyController : IBaseReadOnlyController<Product, ProductOutput>
    {
    }
     
    public class ProductReadOnlyController : BaseReadOnlyController<Product, ProductOutput, IProductUseCasesReadOnlyInteractor>, IProductReadOnlyController
    {
        public ProductReadOnlyController(IProductUseCasesReadOnlyInteractor useCaseReadOnlyInteractor) : base(useCaseReadOnlyInteractor)
        {
        }
    }

    public interface IProductManipulationController : IBaseManipulationController<Product, ProductInput, ProductOutput>, IProductReadOnlyController
    {

    }

    public class ProductManipulationController : BaseManipulationController<Product, ProductInput, ProductOutput, IProductUseCasesManipulationInteractor, IProductReadOnlyController>, IProductManipulationController
    {
        public ProductManipulationController(IProductUseCasesManipulationInteractor useCaseInteractor, IProductReadOnlyController readOnlyController) : base(useCaseInteractor, readOnlyController)
        {
        }
    }
}