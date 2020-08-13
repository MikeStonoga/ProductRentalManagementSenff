using PRM.Domain.Products.Rents.Dtos;

namespace PRM.InterfaceAdapters.Controllers.Products.Dtos.FinishRentDtos
{
    public class FinishRentOutput : RentFinishedResult
    {
        public FinishRentOutput()
        {
            
        }
        public FinishRentOutput(RentFinishedResult result)
        {
            ValueToPay = result.ValueToPay;
            ProductId = result.ProductId;
            UserId = result.UserId;
            DailyPrice = result.DailyPrice;
            StartDate = result.StartDate;
            EndDate = result.EndDate;
            DailyLateFee = result.DailyLateFee;
            WasProductDamaged = result.WasProductDamaged;
            DamageFee = result.DamageFee;
        }
    }
}