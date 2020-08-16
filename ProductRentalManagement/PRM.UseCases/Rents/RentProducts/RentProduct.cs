using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PRM.Domain.BaseCore.Dtos;
using PRM.Domain.BaseCore.ValueObjects;
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
        private readonly IReadOnlyPersistenceGateway<Renter> _renter;
        private readonly IManipulationPersistenceGateway<RenterRentalHistory> _renterRentalHistories;
        private readonly IManipulationPersistenceGateway<Product> _products; 
        private readonly IManipulationPersistenceGateway<ProductRentalHistory> _productRentalHistories;


        public RentProducts(IManipulationPersistenceGateway<Rent> rents, IManipulationPersistenceGateway<RenterRentalHistory> renterRentalHistories, IManipulationPersistenceGateway<Product> products, IManipulationPersistenceGateway<ProductRentalHistory> productRentalHistories, IReadOnlyPersistenceGateway<Renter> renter)
        {
            _rents = rents;
            _renterRentalHistories = renterRentalHistories;
            _products = products;
            _productRentalHistories = productRentalHistories;
            _renter = renter;
        }

        public override async Task<UseCaseResult<RentProductsResult>> Execute(RentProductsRequirement rentProductsRequirement)
        {
            var validationResponse = await Validate(rentProductsRequirement);
            if (!validationResponse.Success) return UseCasesResponses.ExecutionFailure<RentProductsResult>(validationResponse.Message);
            
            var rentToCreate = new Rent(validationResponse.Result.RentPeriod, validationResponse.Result.Products, validationResponse.Result.Renter);
                
            
            var rentProductsResponse = rentToCreate.RentProducts();
            if (!rentProductsResponse.Success) return UseCasesResponses.ExecutionFailure<RentProductsResult>(rentProductsResponse.Message);
            
            
            var rentCreated = await Persist(rentProductsResponse, validationResponse);
            
            var rentProductsResult = new RentProductsResult(rentCreated);
            return UseCasesResponses.SuccessfullyExecuted(rentProductsResult);
        }
        
        private async Task<UseCaseResult<RentProductsValidationResult>> Validate(RentProductsRequirement rentProductsRequirement)
        {
            var isTryingToRentWithoutProducts = rentProductsRequirement.ProductsIds == null; 
            if (isTryingToRentWithoutProducts) return UseCasesResponses.ExecutionFailure<RentProductsValidationResult>("Trying to Rent without products");
            
            var productsToRent = await _products.GetByIds(rentProductsRequirement.ProductsIds.ToList());
            if (!productsToRent.Success) return UseCasesResponses.ExecutionFailure<RentProductsValidationResult>(productsToRent.Message);
            
            var renter = await _renter.GetById(rentProductsRequirement.RenterId);
            if (!renter.Success) return UseCasesResponses.ExecutionFailure<RentProductsValidationResult>(renter.Message);
            
            var rentPeriod = DateRangeProvider.GetDateRange(rentProductsRequirement.StartDate, rentProductsRequirement.EndDate);
            if (!rentPeriod.Success) return UseCasesResponses.ExecutionFailure<RentProductsValidationResult>(rentPeriod.Message);
            
            var validationResult = new RentProductsValidationResult
            {
                Products = productsToRent.Response,
                RentPeriod = rentPeriod.Result,
                Renter = renter.Response
            };

            return UseCasesResponses.SuccessfullyExecuted(validationResult);
        }
        
        private async Task<Rent> Persist(DomainResponseDto<Rent> rentProductsResponse, UseCaseResult<RentProductsValidationResult> validationResponse)
        {
            // TODO: UnitOfWork
            var rentCreatedResponse = await _rents.Create(rentProductsResponse.Result);
            await _renterRentalHistories.Create(new RenterRentalHistory(rentProductsResponse.Result.Id, validationResponse.Result.Renter.Id));
            
            foreach (var product in validationResponse.Result.Products)
            {
                product.MarkAsUnavailable();
                await _products.Update(product);
                await _productRentalHistories.Create(new ProductRentalHistory(rentProductsResponse.Result.Id, product.Id));
            }

            return rentCreatedResponse.Response;
        }
    }

    internal class RentProductsValidationResult
    {
        public DateRange RentPeriod { get; set; }
        public Renter Renter { get; set; }
        public List<Product> Products { get; set; }
    }
}