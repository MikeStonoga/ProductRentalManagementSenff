using System;

namespace PRM.Domain.Rents.Dtos
{
    public class RentFinishedResult : Rent
    {
        public decimal ValueToPay { get; set; }
    }
}