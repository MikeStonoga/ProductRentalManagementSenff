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
        public RentProductsInput(List<Guid> productsIds, Guid renterId, DateTime startDate, DateTime endDate) : base(productsIds, renterId, startDate, endDate)
        {
        }
    }
}