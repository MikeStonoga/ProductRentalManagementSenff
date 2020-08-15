using System;
using System.Collections.Generic;
using PRM.Domain.Rents.Dtos;

namespace PRM.UseCases.Rents.RentProducts
{
    public class RentProductsRequirement : RentRequirement
    {
        public RentProductsRequirement(List<Guid> productsIds, Guid renterId, decimal dailyPrice, DateTime startDate, DateTime endDate, decimal dailyLateFee, string name) : base(productsIds, renterId, dailyPrice, startDate, endDate, dailyLateFee, name)
        {
        }
    }
}