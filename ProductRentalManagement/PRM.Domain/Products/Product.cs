using PRM.Domain.BaseCore;
using PRM.Domain.Rents.Enums;

namespace PRM.Domain.Products
{
    public class Product : FullAuditedEntity
    {
        #region Properties
        public ProductRentalHistory ProductRentalHistory { get; set; }
        public string Description { get; set; }
        public RentStatus Status { get;  set; }
        public bool IsAvailable => Status == RentStatus.Available;
        #endregion

        #region Methods

        public void MarkAsAvailable()
        {
            Status = RentStatus.Available;
        }

        public void MarkAsUnavailable()
        {
            Status = RentStatus.Unavailable;
        }
        
        #endregion
        
    }
}