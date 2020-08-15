using PRM.Domain.Renters;

namespace PRM.InterfaceAdapters.Controllers.Renters.Dtos
{
    public class RenterOutput : Renter
    {

        public RenterOutput()
        {
            
        }
        
        public RenterOutput(Renter product)
        {
            Id = product.Id;
            Code = product.Code;
            Name = product.Name;
            CreationTime = product.CreationTime;
            CreatorId = product.CreatorId;
            DeleterId = product.DeleterId;
            DeletionTime = product.DeletionTime;
            IsDeleted = product.IsDeleted;
            LastModificationTime = product.LastModificationTime;
            LastModifierId = product.LastModifierId;            
        }
    }
}