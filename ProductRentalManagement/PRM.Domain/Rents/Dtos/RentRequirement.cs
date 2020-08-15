using System;
using System.Collections.Generic;

namespace PRM.Domain.Rents.Dtos
{
    public class RentRequirement
    {
        public List<Guid> ProductsIds { get; set; } 
        public Guid RenterId { get; set; }
        public decimal DailyPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DailyLateFee { get; set; }
        public string Name { get; set; }
    }
}