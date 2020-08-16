using System.Linq;
using System.Threading.Tasks;
using PRM.Domain.BaseCore.ValueObjects;
using PRM.Domain.Products;
using PRM.Domain.Renters;
using PRM.Domain.Rents.Dtos;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;
using PRM.UseCases.BaseCore.Validations;
using PRM.UseCases.Rents.RentProducts;

namespace PRM.UseCases.Rents.Validations.Requirements.Rents
{
    public interface IValidateRentRequirement : IValidateUseCaseRequirement<RentRequirement, RentRequirementValidationResult>
    {
    }
    public class ValidateRentRequirement : UseCaseRequirementValidation<RentRequirement, RentRequirementValidationResult>, IValidateRentRequirement
    {
        private readonly IReadOnlyPersistenceGateway<Renter> _renter;
        private readonly IReadOnlyPersistenceGateway<Product> _products; 

        public ValidateRentRequirement(IReadOnlyPersistenceGateway<Renter> renter, IReadOnlyPersistenceGateway<Product> products)
        {
            _renter = renter;
            _products = products;
        }

        public override async Task<UseCaseResult<RentRequirementValidationResult>> Validate(RentRequirement rentProductsRequirement)
        {
            var isTryingToRentWithoutProducts = rentProductsRequirement.ProductsIds == null; 
            if (isTryingToRentWithoutProducts) return UseCasesResponses.ExecutionFailure<RentRequirementValidationResult>("Trying to Rent without products");
            
            var productsToRent = await _products.GetByIds(rentProductsRequirement.ProductsIds.ToList());
            if (!productsToRent.Success) return UseCasesResponses.ExecutionFailure<RentRequirementValidationResult>(productsToRent.Message);
            
            var renter = await _renter.GetById(rentProductsRequirement.RenterId);
            if (!renter.Success) return UseCasesResponses.ExecutionFailure<RentRequirementValidationResult>(renter.Message);
            
            var rentPeriod = DateRangeProvider.GetDateRange(rentProductsRequirement.StartDate, rentProductsRequirement.EndDate);
            if (!rentPeriod.Success) return UseCasesResponses.ExecutionFailure<RentRequirementValidationResult>(rentPeriod.Message);
            
            var validationResult = new RentRequirementValidationResult
            {
                Products = productsToRent.Response,
                RentPeriod = rentPeriod.Result,
                Renter = renter.Response
            };

            return UseCasesResponses.SuccessfullyExecuted(validationResult);
        }
    }
}