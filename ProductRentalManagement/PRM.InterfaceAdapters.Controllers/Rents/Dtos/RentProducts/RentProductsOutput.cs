using PRM.Domain.Rents.Dtos;

namespace PRM.InterfaceAdapters.Controllers.Rents.Dtos.RentProducts
{
    public class RentProductsOutput : RentProductsResult
    {
        public RentProductsOutput()
        {
            
        }

        public RentProductsOutput(RentProductsResult rentProductsResult)
        {
            Id = rentProductsResult.Id;
            Code = rentProductsResult.Code;
            Name = rentProductsResult.Name;
            RenterId = rentProductsResult.RenterId;
            StartDate = rentProductsResult.StartDate;
            EndDate = rentProductsResult.EndDate;
            DailyPrice = rentProductsResult.DailyPrice;
            DailyLateFee = rentProductsResult.DailyLateFee;
            WasProductDamaged = rentProductsResult.WasProductDamaged;
            DamageFee = rentProductsResult.DamageFee;
        }
    }
}