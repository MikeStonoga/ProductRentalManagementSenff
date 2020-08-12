using System;

namespace PRM.Domain.Products.Rents.Dtos
{
    public class RentRequirement
    {
        public Guid ProductId { get; set; }
        public Guid RenterId { get; set; }
        public decimal DailyPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DailyLateFee { get; set; }
        public string Name { get; set; }
    }
}