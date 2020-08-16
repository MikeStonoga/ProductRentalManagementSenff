using System.Linq;
using System.Threading.Tasks;
using PRM.Domain.Products;
using PRM.Domain.Renters;
using PRM.Domain.Rents;
using PRM.Domain.Rents.Dtos;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;

namespace PRM.UseCases.Rents.RentProducts
{
    public interface IRentProducts : IBaseUseCase<RentProductsRequirement, RentProductsResult>
    {
    }
    
    public class RentProducts : BaseUseCase<RentProductsRequirement, RentProductsResult>, IRentProducts
    {
        private readonly IManipulationPersistenceGateway<Rent> _rents;
        private readonly IManipulationPersistenceGateway<RenterRentalHistory> _renterRentalHistories;
        private readonly IManipulationPersistenceGateway<Product> _products; 
        private readonly IManipulationPersistenceGateway<ProductRentalHistory> _productRentalHistories;


        public RentProducts(IManipulationPersistenceGateway<Rent> rents, IManipulationPersistenceGateway<RenterRentalHistory> renterRentalHistories, IManipulationPersistenceGateway<Product> products, IManipulationPersistenceGateway<ProductRentalHistory> productRentalHistories)
        {
            _rents = rents;
            _renterRentalHistories = renterRentalHistories;
            _products = products;
            _productRentalHistories = productRentalHistories;
        }

        public override async Task<UseCaseResult<RentProductsResult>> Execute(RentProductsRequirement rentProductsRequirement)
        {
            if (rentProductsRequirement.ProductsIds == null) return UseCasesResponses.ExecutionFailureResponse<RentProductsResult>("Trying to Rent without products");
            
            var productsToRentResponse = await _products.GetByIds(rentProductsRequirement.ProductsIds.ToList());
            if (!productsToRentResponse.Success) return UseCasesResponses.ExecutionFailureResponse<RentProductsResult>(productsToRentResponse.Message);

            var rentToCreate = new Rent(rentProductsRequirement, productsToRentResponse.Response);
            var rentProductsResponse = rentToCreate.RentProducts();
            if (!rentProductsResponse.Success) return UseCasesResponses.ExecutionFailureResponse<RentProductsResult>(rentProductsResponse.Message);
            
            // TODO: UnitOfWork
            var rentCreatedResponse = await _rents.Create(rentProductsResponse.Result);
            await _renterRentalHistories.Create(new RenterRentalHistory(rentProductsResponse.Result.Id, rentProductsRequirement.RenterId));
            
            foreach (var product in productsToRentResponse.Response)
            {
                product.MarkAsUnavailable();
                await _products.Update(product);
                await _productRentalHistories.Create(new ProductRentalHistory(rentProductsResponse.Result.Id, product.Id));
            }
            
            var rentProductsResult = new RentProductsResult(rentCreatedResponse.Response);
            return UseCasesResponses.SuccessfullyExecutedResponse(rentProductsResult);
        }
    }
}