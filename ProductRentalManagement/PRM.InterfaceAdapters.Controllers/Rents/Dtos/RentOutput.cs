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
            RenterRentalHistory = rent.RenterRentalHistory;
            ProductRentalHistories = rent.ProductRentalHistories;
            RenterId = rent.RenterId;
            Products = rent.Products;
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