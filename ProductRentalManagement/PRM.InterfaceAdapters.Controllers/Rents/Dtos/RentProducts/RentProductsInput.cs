using System;
using System.Collections.Generic;
using PRM.UseCases.Rents.RentProducts;

namespace PRM.InterfaceAdapters.Controllers.Rents.Dtos.RentProducts
{
    public class RentProductsInput : RentProductsRequirement
    {
        public RentProductsInput()
        {
            
        }
        public RentProductsInput(List<Guid> productsIds, Guid renterId, decimal dailyPrice, DateTime startDate, DateTime endDate, decimal dailyLateFee, string name) : base(productsIds, renterId, dailyPrice, startDate, endDate, dailyLateFee, name)
        {
        }
    }
}