using System;

namespace PRM.UseCases.Rents.FinishRents
{
    public class FinishRentRequirement
    {
        public Guid RentId { get; }
        public decimal DamageFee { get; }
        public decimal Discount { get; }
        
        public FinishRentRequirement(Guid rentId, decimal damageFee, decimal discount)
        {
            RentId = rentId;
            DamageFee = damageFee;
            Discount = discount;
        }
    }
}