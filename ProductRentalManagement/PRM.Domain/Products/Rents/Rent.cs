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
        public bool WasProductDamaged { get; set; }
        public decimal DamageFee { get; set; }

        public decimal CurrentRentPaymentValue => PriceWithoutFees + LateFee + DamageFee;
        public decimal PriceWithoutFees => DailyPrice * RentDays;

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
            CreationTime = DateTime.Now;
            DailyLateFee = rentRequirement.DailyLateFee;
        }

        public RentFinishedResult FinishRent()
        {
            return new RentFinishedResult
            {
                UserId = UserId,
                ProductId = ProductId,
                StartDate = StartDate,
                EndDate = EndDate,
                DailyPrice = DailyPrice,
                DailyLateFee = DailyLateFee,
                DamageFee = DamageFee,
                ValueToPay = CurrentRentPaymentValue
            };
        }
    }
}