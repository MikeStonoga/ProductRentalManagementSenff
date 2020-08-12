using System;

namespace PRM.Domain.Products.Rents.Dtos
{
    public class RentResult
    {
        public string ProductDescription { get; set; }

        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public decimal DailyPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DailyLateFee { get; set; }
        
        public string Name { get; set; }
        public Guid? LastModifierId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
        public decimal CurrentRentPaymentValue { get; set; }

        public RentResult()
        {
            
        }
        
        public RentResult(Rent rent, string productDescription)
        {
            ProductDescription = productDescription;
            ProductId = rent.ProductId;
            DailyPrice = rent.DailyPrice;
            EndDate = rent.EndDate;
            StartDate = rent.StartDate;
            UserId = rent.UserId;
            DailyLateFee = rent.DailyLateFee;
            Name = rent.Name;
            CreationTime = rent.CreationTime;
            CreatorId = rent.CreatorId;
            LastModificationTime = rent.LastModificationTime;
            LastModifierId = rent.LastModifierId;
            CurrentRentPaymentValue = rent.CurrentRentPaymentValue;
        }

    }
}