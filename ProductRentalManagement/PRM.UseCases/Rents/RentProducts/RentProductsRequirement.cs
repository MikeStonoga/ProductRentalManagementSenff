using System;
using System.Collections.Generic;
using PRM.Domain.Rents.Dtos;

namespace PRM.UseCases.Rents.RentProducts
{
    public class RentProductsRequirement : RentRequirement
    {
        public RentProductsRequirement()
        {
            
        }
        public RentProductsRequirement(List<Guid> productsIds, Guid renterId, DateTime startDate, DateTime endDate) : base(productsIds, renterId, startDate, endDate)
        {
        }
    }
}