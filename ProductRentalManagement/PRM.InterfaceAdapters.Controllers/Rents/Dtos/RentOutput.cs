using PRM.Domain.Rents;

namespace PRM.InterfaceAdapters.Controllers.Rents.Dtos
{
    public class RentOutput : Rent
    {

        public RentOutput()
        {
            
        }
        
        public RentOutput(Rent rent)
        {
            Id = rent.Id;
            Code = rent.Code;
            Name = rent.Name;
            CreationTime = rent.CreationTime;
            CreatorId = rent.CreatorId;
            DeleterId = rent.DeleterId;
            DeletionTime = rent.DeletionTime;
            IsDeleted = rent.IsDeleted;
            LastModificationTime = rent.LastModificationTime;
            LastModifierId = rent.LastModifierId;
            RenterId = rent.RenterId;
            Products = rent.Products;
            StartDate = rent.StartDate;
            EndDate = rent.EndDate;
            DailyPrice = rent.DailyPrice;
            DailyLateFee = rent.DailyLateFee;
            WasProductDamaged = rent.WasProductDamaged;
            DamageFee = rent.DamageFee;
        }
    }
}