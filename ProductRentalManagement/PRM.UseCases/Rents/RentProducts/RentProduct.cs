using System.Threading.Tasks;
using PRM.Domain.Products;
using PRM.Domain.Renters;
using PRM.Domain.Rents;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;

namespace PRM.UseCases.Rents.RentProducts
{
    public interface IRentProduct : IBaseUseCase<RentProductRequeriment, Rent>
    {
        
    }
    
    public class RentProduct : BaseUseCase<RentProductRequeriment, Rent>, IRentProduct
    {
        private readonly IManipulationPersistenceGateway<Rent> _rents;
        private readonly IManipulationPersistenceGateway<RenterRentalHistory> _renterRentalHistories;
        private readonly IManipulationPersistenceGateway<Product> _products; 
        private readonly IManipulationPersistenceGateway<ProductRentalHistory> _productRentalHistories;


        public RentProduct(IManipulationPersistenceGateway<Rent> rents, IManipulationPersistenceGateway<RenterRentalHistory> renterRentalHistories, IManipulationPersistenceGateway<Product> products, IManipulationPersistenceGateway<ProductRentalHistory> productRentalHistories)
        {
            _rents = rents;
            _renterRentalHistories = renterRentalHistories;
            _products = products;
            _productRentalHistories = productRentalHistories;
        }

        public override async Task<UseCaseResult<Rent>> Execute(RentProductRequeriment rentProductsRequirement)
        {
            var productsToRentResponse = await _products.GetByIds(rentProductsRequirement.ProductsIds);
            if (!productsToRentResponse.Success) return UseCasesResponses.ExecutionFailureResponse<Rent>(productsToRentResponse.Message);


            var rentProductsResponse = new Rent(rentProductsRequirement, productsToRentResponse.Response).RentProducts();
            if (!rentProductsResponse.Success) return UseCasesResponses.ExecutionFailureResponse<Rent>(rentProductsResponse.Message);

            await _renterRentalHistories.Create(rentProductsResponse.Result.RenterRentalHistory);
            
            foreach (var product in rentProductsResponse.Result.Products)
            {
                await _products.Update(product);
                await _productRentalHistories.Create(product.ProductRentalHistory);
            }

            var rentCreatedResponse = await _rents.Create(rentProductsResponse.Result);
            return UseCasesResponses.SuccessfullyExecutedResponse(rentCreatedResponse.Response);
        }
    }
}