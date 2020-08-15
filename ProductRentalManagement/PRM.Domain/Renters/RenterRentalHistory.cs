using System;
using PRM.Domain.BaseCore;

namespace PRM.Domain.Renters
{
    public class RenterRentalHistory : FullAuditedEntity
    {
        #region Properties
        public Guid RentId { get; }
        public Guid RenterId { get; }

        #endregion

        #region Constructors

        public RenterRentalHistory()
        {
            
        }
        
        public RenterRentalHistory(Guid rentId, Guid renterId)
        {
            RentId = rentId;
            RenterId = renterId;
        }

        #endregion
    }
}