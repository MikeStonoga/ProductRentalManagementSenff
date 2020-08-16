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
            RenterId = rent.RenterId;
            Status = rent.Status;
            StartDate = rent.StartDate;
            EndDate = rent.EndDate;
            DailyPrice = rent.DailyPrice;
            DailyLateFee = rent.DailyLateFee;
            WasProductDamaged = rent.WasProductDamaged;
            DamageFee = rent.DamageFee;
            Discount = rent.Discount;
        }
        
    }
}