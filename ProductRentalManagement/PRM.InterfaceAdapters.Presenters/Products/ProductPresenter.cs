using PRM.Domain.Products;
using PRM.InterfaceAdapters.Presenters.BaseCore;

namespace PRM.InterfaceAdapters.Presenters.Products
{
    public interface IProductReadOnlyPresenter : IBaseReadOnlyPresenter<Product, ProductView>
    {
    }
    
    public abstract class ProductReadOnlyPresenter : BaseReadOnlyPresenter<Product, ProductView>, IProductReadOnlyPresenter
    {
    }
    
    public interface IProductManipulationPresenter : IBaseManipulationPresenter<Product, ProductView>, IProductReadOnlyPresenter
    {
    }

    public abstract class ProductManipulationPresenter : BaseManipulationPresenter<Product, ProductView, IProductReadOnlyPresenter>, IProductManipulationPresenter
    {
        public ProductManipulationPresenter(IProductReadOnlyPresenter readOnlyPresenter) : base(readOnlyPresenter)
        {
        }
    }
}