using System;
using System.Collections.Generic;
using PRM.Domain.Rents.Dtos;

namespace PRM.UseCases.Rents.GetRentForecastPrices
{
    public class GetRentForecastPriceRequirement
    {
        #region Properties
        public RentRequirement RentRequirement { get; set; }
        public List<Guid> ProductsIds { get; set; }
        #endregion

        #region Constructors

        public GetRentForecastPriceRequirement()
        {
            
        }

        public GetRentForecastPriceRequirement(RentRequirement rentRequirement, List<Guid> productsIds)
        {
            RentRequirement = rentRequirement ?? throw new ArgumentNullException(nameof(rentRequirement));
            ProductsIds = productsIds ?? throw new ArgumentException(nameof(productsIds));
        }
        #endregion
        
    }
}