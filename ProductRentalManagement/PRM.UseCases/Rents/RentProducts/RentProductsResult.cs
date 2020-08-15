namespace PRM.Domain.Rents.Dtos
{
    public class RentProductsResult : Rent
    {
        public RentProductsResult()
        {
            
        }
        
        public RentProductsResult(Rent rent)
        {
            Id = rent.Id;
            Code = rent.Code;
            Name = rent.Name;
            RenterRentalHistory = rent.RenterRentalHistory;
            ProductRentalHistories = rent.ProductRentalHistories;
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