using System;

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
            RenterId = rent.RenterId;
            RentPeriod = rent.RentPeriod;
            DailyPrice = rent.DailyPrice;
            DailyLateFee = rent.DailyLateFee;
            WasProductDamaged = rent.WasProductDamaged;
            DamageFee = rent.DamageFee;
        }

    }
}