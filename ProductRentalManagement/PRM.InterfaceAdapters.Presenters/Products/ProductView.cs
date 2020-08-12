using System.Collections.Generic;
using PRM.Domain.Products;
using PRM.InterfaceAdapters.Presenters.BaseCore.Dtos;
using PRM.InterfaceAdapters.Presenters.Products.Rents;

namespace PRM.InterfaceAdapters.Presenters.Products
{
    public class ProductView : FullAuditedEntityView<Product>
    {
        public string Description { get; set; }
        public string StatusName { get; set; }
        public List<RentView> Rents { get; set; }

        public ProductView()
        {
            
        }
        
        public ProductView(Product entity) : base(entity)
        {
            Description = entity.Description;
            StatusName = entity.Status.ToString();

            var rentViews = new List<RentView>();
        }
    }
}