using System;
using System.Collections.Generic;

namespace PRM.Domain.Rents.Dtos
{
    public class RentRequirement
    {
        public RentRequirement()
        {
            
        }
        public RentRequirement(List<Guid> productsIds, Guid renterId, decimal dailyPrice, DateTime startDate, DateTime endDate, decimal dailyLateFee, string name)
        {
            ProductsIds = productsIds;
            RenterId = renterId;
            DailyPrice = dailyPrice;
            StartDate = startDate;
            EndDate = endDate;
            DailyLateFee = dailyLateFee;
            Name = name;
        }

        public List<Guid> ProductsIds { get; } 
        public Guid RenterId { get; }
        public decimal DailyPrice { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public decimal DailyLateFee { get; }
        public string Name { get; }
    }
}