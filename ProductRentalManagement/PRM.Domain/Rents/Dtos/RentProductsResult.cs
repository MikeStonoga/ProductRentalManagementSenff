using System;
using System.Collections.Generic;
using PRM.Domain.Products;
using PRM.Domain.Renters;

namespace PRM.Domain.Rents.Dtos
{
    public class RentProductsResult
    {
        public List<Product> Products { get; set; }
        public Guid RenterId { get; set; }
        public decimal DailyPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DailyLateFee { get; set; }
        
        public RenterRentalHistory RenterRentalHistory { get; set; }
        public List<ProductRentalHistory> ProductRentalHistories { get; set; }
        
        public string Name { get; set; }
        public Guid? LastModifierId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
        public decimal CurrentRentPaymentValue { get; set; }

        public RentProductsResult()
        {
            
        }
        
        public RentProductsResult(Rent rent)
        {
            Products = rent.Products;
            RenterId = rent.RenterId;
            ProductRentalHistories = rent.ProductRentalHistories;
            RenterRentalHistory = rent.RenterRentalHistory;
            DailyPrice = rent.DailyPrice;
            EndDate = rent.EndDate;
            StartDate = rent.StartDate;
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