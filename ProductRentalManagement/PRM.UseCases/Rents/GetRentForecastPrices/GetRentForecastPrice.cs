using System;
using System.Threading.Tasks;
using PRM.Domain.BaseCore.ValueObjects;
using PRM.Domain.Products;
using PRM.Domain.Renters;
using PRM.Domain.Rents;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;

namespace PRM.UseCases.Rents.GetRentForecastPrices
{
    public interface IGetRentForecastPrice : IBaseUseCase<GetRentForecastPriceRequirement, GetRentForecastPriceResult>
    {
    }
    
    public class GetRentForecastPrice : BaseUseCase<GetRentForecastPriceRequirement, GetRentForecastPriceResult>, IGetRentForecastPrice
    {
        private readonly IReadOnlyPersistenceGateway<Product> _products;
        private readonly IReadOnlyPersistenceGateway<Renter> _renters;

        public GetRentForecastPrice(IReadOnlyPersistenceGateway<Product> products, IReadOnlyPersistenceGateway<Renter> renters)
        {
            _products = products;
            _renters = renters;
        }

        public override async Task<UseCaseResult<GetRentForecastPriceResult>> Execute(GetRentForecastPriceRequirement rentProductsRequirement)
        {
            var isTryingToCalculateWithoutProducts = rentProductsRequirement.ProductsIds == null;
            if (isTryingToCalculateWithoutProducts) return UseCasesResponses.ExecutionFailure<GetRentForecastPriceResult>("Trying to calculate without any product");
            
            var productsToRentResponse = await _products.GetByIds(rentProductsRequirement.ProductsIds);
            if (!productsToRentResponse.Success) return UseCasesResponses.ExecutionFailure<GetRentForecastPriceResult>(productsToRentResponse.Message);

            var renter = await _renters.GetById(rentProductsRequirement.RenterId);
            if (!renter.Success) return UseCasesResponses.ExecutionFailure<GetRentForecastPriceResult>(renter.Message);
            
                        
            var rentPeriod = DateRangeProvider.GetDateRange(rentProductsRequirement.StartDate, rentProductsRequirement.EndDate);
            if (!rentPeriod.Success) return UseCasesResponses.ExecutionFailure<GetRentForecastPriceResult>(rentPeriod.Message);
            
            var rentForecastPrice = new Rent(rentPeriod.Result, productsToRentResponse.Response, renter.Response).GetRentForecastPrice();
            
            var rentForecastPriceResult =new GetRentForecastPriceResult(rentForecastPrice);
            return UseCasesResponses.SuccessfullyExecuted(rentForecastPriceResult);
        }
    }
}