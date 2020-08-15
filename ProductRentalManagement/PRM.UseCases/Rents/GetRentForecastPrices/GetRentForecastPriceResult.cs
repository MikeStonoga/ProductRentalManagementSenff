namespace PRM.UseCases.Rents.GetRentForecastPrices
{
    public class GetRentForecastPriceResult
    {
        public decimal RentForecastPrice { get; }
        
        public GetRentForecastPriceResult()
        {
            RentForecastPrice = 0;
        }
        
        public GetRentForecastPriceResult(decimal rentForecastPrice)
        {
            RentForecastPrice = rentForecastPrice;
        }
        
    }
}