using Microsoft.AspNetCore.Mvc;
using PRM.Domain.Products;
using PRM.Infrastructure.ApplicationDelivery.WebApiHost.BaseCore;
using PRM.InterfaceAdapters.Controllers.Products;
using PRM.InterfaceAdapters.Controllers.Products.Dtos;
using PRM.UseCases.Products;

namespace PRM.Infrastructure.ApplicationDelivery.WebApiHost.Products
{
    public interface IProductReadOnlyWebController : IProductReadOnlyController, IBaseReadOnlyWebController<Product, ProductOutput>
    {
    }
    
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductReadOnlyController : BaseReadOnlyWebController<Product, ProductOutput, IProductUseCasesReadOnlyInteractor>, IProductReadOnlyWebController
    {
        public ProductReadOnlyController(IProductUseCasesReadOnlyInteractor baseConsoleUseCaseReadOnlyInteractor) : base(baseConsoleUseCaseReadOnlyInteractor)
        {
        }
    }

    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : BaseManipulationWebController<Product, ProductInput, ProductOutput, IProductUseCasesManipulationInteractor, IProductReadOnlyWebController>, IProductManipulationController
    {
        public ProductController(IProductUseCasesManipulationInteractor baseConsoleUseCaseManipulationInteractor, IProductReadOnlyWebController readOnlyController) : base(baseConsoleUseCaseManipulationInteractor, readOnlyController)
        {
        }
    }
}