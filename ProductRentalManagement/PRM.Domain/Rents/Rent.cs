using System;
using System.Collections.Generic;
using System.Linq;
using PRM.Domain.BaseCore;
using PRM.Domain.BaseCore.Dtos;
using PRM.Domain.BaseCore.Extensions;
using PRM.Domain.Products;
using PRM.Domain.Renters;
using PRM.Domain.Rents.Dtos;
using PRM.Domain.Rents.Enums;

namespace PRM.Domain.Rents
{
    public class Rent : FullAuditedEntity
    {
        #region Properties
        public RenterRentalHistory RenterRentalHistory { get; set; }
        public List<ProductRentalHistory> ProductRentalHistories { get; set; }
        public Guid RenterId { get; set; }
        public List<Product> Products { get; set; }
        public RentStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal DailyLateFee { get; set; }
        public bool WasProductDamaged { get; set; }
        public decimal DamageFee { get; set; }
        public decimal Discount { get; set; }

        public decimal CurrentRentPaymentValue => PriceWithoutFees + LateFee + DamageFee;
        public decimal PriceWithoutFees => DailyPrice * RentDays + Discount;

        public int RentDays => EndDate.Subtract(StartDate).Days;

        public decimal LateFee => IsLate ? DailyLateFee * LateDays : 0;
        public bool IsLate => DateTime.Now > EndDate;
        public int LateDays => DateTime.Now.Subtract(EndDate).Days;

        #endregion

        #region Constructors
        
        public Rent()
        {
        }
        
        public Rent(RentRequirement rentRequirement, List<Product> productsToRent)
        {
            if (productsToRent == null) throw new ArgumentException("Trying to create a Rent without any Products");
            if (!productsToRent.All(product => product.IsAvailable)) throw new ArgumentException(GetUnavailableProductsMessage(productsToRent));
            if (rentRequirement.EndDate < rentRequirement.StartDate) throw  new ArgumentException("Rent end date may not be earlier then start date");
            
            Name = rentRequirement.Name;
            Status = RentStatus.Open;
            DailyPrice = productsToRent.Sum(p => (p.RentDailyPrice));
            StartDate = rentRequirement.StartDate;
            EndDate = rentRequirement.EndDate;
            CreationTime = DateTime.Now;
            DailyLateFee = productsToRent.Sum(p => p.RentDailyLateFee);
            RenterId = rentRequirement.RenterId;
            Products = productsToRent;
        }

        private string GetUnavailableProductsMessage(List<Product> productsToRent)
        {
            var unavailableProducts = productsToRent.Where(p => !p.IsAvailable).ToList();
            var exceptionMessage = "Trying to rent unavailable products:";
                
            foreach (var product in unavailableProducts)
            {
                exceptionMessage += $"\n {product.Code} - {product.Name} - {product.Description} - {product.Status}";
            }

            return exceptionMessage;
        }

        #endregion

        #region Methods
        
        public decimal GetRentForecastPrice()
        {
            return PriceWithoutFees;
        }
        
        public DomainResponseDto<Rent> RentProducts()
        {
            RenterRentalHistory = new RenterRentalHistory(Id, RenterId);
            ProductRentalHistories = Products.Select(product => new ProductRentalHistory(Id, product.Id)).ToList();

            foreach (var product in Products)
            {
                product.MarkAsUnavailable();
            }
            
            return this.GetSuccessResponse("Rented");
        }

        public DomainResponseDto<Rent> FinishRent(decimal damageFee = 0, decimal discount = 0)
        {
            if (damageFee != 0M)
            {
                WasProductDamaged = true;
                DamageFee = damageFee;
                Discount = discount;
            }

            foreach (var product in Products)
            {
                product.MarkAsAvailable();
            }

            Status = RentStatus.Closed;
            
            return this.GetSuccessResponse("RentFinished");
        }
        #endregion
    }
    
}