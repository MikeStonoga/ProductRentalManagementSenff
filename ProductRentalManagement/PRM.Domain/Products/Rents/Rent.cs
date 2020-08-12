using System;
using PRM.Domain.BaseCore;
using PRM.Domain.Products.Rents.Dtos;

namespace PRM.Domain.Products.Rents
{
    public class Rent : FullAuditedEntity
    {
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }
        public decimal DailyPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DailyLateFee { get; set; }

        public decimal CurrentRentPaymentValue => PriceWithoutLateFee + LateFee;
        public decimal PriceWithoutLateFee => DailyPrice * RentDays;
        public int RentDays => EndDate.Subtract(StartDate).Days;

        public decimal LateFee => IsLate ? DailyLateFee * LateDays : 0;
        public bool IsLate => DateTime.Now > EndDate;
        public int LateDays => DateTime.Now.Subtract(EndDate).Days;

        
        public Rent()
        {
            
        }
        
        public Rent(RentRequirement rentRequirement)
        {
            Name = rentRequirement.Name;
            DailyPrice = rentRequirement.DailyPrice;
            ProductId = rentRequirement.ProductId;
            StartDate = rentRequirement.StartDate;
            EndDate = rentRequirement.EndDate;
            UserId = rentRequirement.RenterId;
            DailyLateFee = rentRequirement.DailyLateFee;
        }

        public RentFinishedDto FinishRent()
        {
            return new RentFinishedDto
            {
                UserId = UserId,
                ProductId = ProductId,
                StartDate = StartDate,
                EndDate = EndDate,
                DailyPrice = DailyPrice,
                DailyLateFee = DailyLateFee,
                ValueToPay = CurrentRentPaymentValue
            };
        }
    }
}