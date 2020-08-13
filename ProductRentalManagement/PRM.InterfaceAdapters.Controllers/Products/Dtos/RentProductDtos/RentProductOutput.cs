using PRM.Domain.Products.Rents.Dtos;

namespace PRM.InterfaceAdapters.Controllers.Products.Dtos.RentProductDtos
{
    public class RentProductOutput : RentResult
    {
        public RentProductOutput()
        {
            
        }
        
        public RentProductOutput(RentResult result)
        {
            ProductId = result.ProductId;
            ProductDescription = result.ProductDescription;
            UserId = result.UserId;
            DailyPrice = result.DailyPrice;
            StartDate = result.StartDate;
            EndDate = result.EndDate;
            DailyLateFee = result.DailyLateFee;
            Name = result.Name;
            LastModifierId = result.LastModifierId;
            LastModificationTime = result.LastModificationTime;
            CreatorId = result.CreatorId;
            CreationTime = result.CreationTime;
            CurrentRentPaymentValue = result.CurrentRentPaymentValue;
        }
    }
}