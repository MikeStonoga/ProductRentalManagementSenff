using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PRM.Domain.BaseCore;
using PRM.Domain.BaseCore.Dtos;
using PRM.Domain.BaseCore.Extensions;
using PRM.Domain.BaseCore.ValueObjects;
using PRM.Domain.Products;
using PRM.Domain.Products.Extensions;
using PRM.Domain.Renters;
using PRM.Domain.Rents.Enums;

namespace PRM.Domain.Rents
{
    public class Rent : FullAuditedEntity
    {
        #region Properties
        public Guid RenterId { get; set; }
        public RentStatus Status { get; set; }
        
        public DateRange RentPeriod { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal DailyLateFee { get; set; }
        public bool WasProductDamaged { get; set; }
        public decimal DamageFee { get; set; }
        public decimal Discount { get; set; }

        public decimal CurrentRentPaymentValue => PriceWithoutFees + LateFee + DamageFee;
        public decimal PriceWithoutFees => DailyPrice * RentDays + Discount;

        public int RentDays => RentPeriod.Days;

        public decimal LateFee => IsLate ? DailyLateFee * LateDays : 0;
        public bool IsLate => DateTime.Now > RentPeriod.EndDate;
        public int LateDays => DateTime.Now.Subtract(RentPeriod.EndDate).Days;
        public bool IsFinished => Status == RentStatus.Closed;

        #endregion

        #region Constructors
        public Rent()
        {
        }
        
        public Rent(DateRange rentPeriod, List<Product> productsToRent, Renter renter)
        {
            if (productsToRent == null) throw new ValidationException("Trying to create a Rent without any Products");

            bool IsUnavailableProduct(Product product) => !product.IsAvailable;
            var hasUnavailableProduct = productsToRent.Any(IsUnavailableProduct); 
            if (hasUnavailableProduct) throw new ValidationException(productsToRent.GetProductsWithErrorMessage(IsUnavailableProduct, "Trying to rent unavailable products:"));

            Name = "Created: " + DateTime.Now.FormatDate() + " - Start: " + rentPeriod.StartDate.FormatDate() + " - End: " + rentPeriod.EndDate.FormatDate();
            DailyPrice = productsToRent.Sum(p => (p.RentDailyPrice));
            RentPeriod = rentPeriod;
            CreationTime = DateTime.Now;
            DailyLateFee = productsToRent.Sum(p => p.RentDailyLateFee);
            RenterId = renter.Id;
        }
        #endregion

        #region Methods
        
        public decimal GetRentForecastPrice()
        {
            return PriceWithoutFees;
        }
        
        public DomainResponseDto<Rent> RentProducts()
        {
            Status = RentStatus.Open;
            return this.GetSuccessResponse("Rented");
        }

        public DomainResponseDto<Rent> FinishRent(decimal damageFee = 0, decimal discount = 0)
        {
            if (IsFinished) return DomainValidationsExtensions.GetFailureResponse<Rent>("Already finished: " + LastModificationTime?.FormatDate());

            var wasProductsDamaged = damageFee != 0M;
            if (wasProductsDamaged)
            {
                WasProductDamaged = true;
                DamageFee = damageFee;
            }

            var hasDiscount = discount != 0M;
            if (hasDiscount)
            {
                Discount = discount;
            }
            
            Status = RentStatus.Closed;
            
            return this.GetSuccessResponse("RentFinished");
        }
        #endregion
    }
    
}