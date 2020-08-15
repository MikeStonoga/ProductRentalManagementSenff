using System;
using PRM.Domain.BaseCore;

namespace PRM.Domain.Products
{
    public class ProductRentalHistory : FullAuditedEntity
    {
        #region Properties
        public Guid RentId { get; }
        public Guid ProductId { get;  }
        #endregion

        #region Constructors

        public ProductRentalHistory()
        {
            
        }
        
        public ProductRentalHistory(Guid rentId, Guid productId)
        {
            RentId = rentId;
            ProductId = productId;
        }
        #endregion
    }
}