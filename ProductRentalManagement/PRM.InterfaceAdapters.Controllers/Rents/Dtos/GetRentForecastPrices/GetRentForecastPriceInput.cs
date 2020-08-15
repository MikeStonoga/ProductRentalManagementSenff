using System;
using System.Collections.Generic;
using PRM.Domain.Rents.Dtos;
using PRM.UseCases.Rents.GetRentForecastPrices;

namespace PRM.InterfaceAdapters.Controllers.Rents.Dtos.GetRentForecastPrices
{
    public class GetRentForecastPriceInput : GetRentForecastPriceRequirement
    {
        public GetRentForecastPriceInput()
        {
            
        }
        public GetRentForecastPriceInput(RentRequirement rentRequirement, List<Guid> productsIds) : base(rentRequirement, productsIds)
        {
        }
    }
}