using PRM.Domain.BaseCore;
using PRM.Domain.Products.Enums;

namespace PRM.Domain.Products
{
    public class Product : FullAuditedEntity
    {
        #region Properties
        public string Description { get; set; }
        public ProductStatus Status { get;  set; }
        public bool IsAvailable => Status == ProductStatus.Available;
        public decimal RentDailyPrice { get; set; }
        public decimal RentDailyLateFee { get; set; }
        #endregion

        #region Methods

        public void MarkAsAvailable()
        {
            Status = ProductStatus.Available;
        }

        public void MarkAsUnavailable()
        {
            Status = ProductStatus.Unavailable;
        }
        
        #endregion
        
    }
}