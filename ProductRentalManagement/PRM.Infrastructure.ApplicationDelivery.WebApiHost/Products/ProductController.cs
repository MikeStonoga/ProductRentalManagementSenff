using PRM.Domain.Products;
using PRM.Infrastructure.ApplicationDelivery.WebApiHost.BaseCore;
using PRM.InterfaceAdapters.Controllers.Products;
using PRM.InterfaceAdapters.Controllers.Products.Dtos;
using PRM.UseCases.Products;

namespace PRM.Infrastructure.ApplicationDelivery.WebApiHost.Products
{

    public class ProductController : BaseManipulationWebController<Product, ProductInput, ProductOutput, IProductUseCasesManipulationInteractor, IProductManipulationController>, IProductManipulationController
    {
        public ProductController(IProductUseCasesManipulationInteractor useCaseInteractor, IProductManipulationController manipulationController) : base(useCaseInteractor, manipulationController)
        {
        }
    }
}