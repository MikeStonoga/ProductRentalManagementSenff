using System;
using System.Threading.Tasks;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;
using PRM.UseCases.Products.CheckAvailabilities;

namespace PRM.UseCases.Products.CheckProductAvailabilities
{
    public interface ICheckProductAvailability : IBaseUseCase<Guid, CheckProductAvailabilityResult>
    {
    }
    
    public class CheckProductAvailability : BaseUseCase<Guid, CheckProductAvailabilityResult>, ICheckProductAvailability
    {
        private readonly IProductUseCasesReadOnlyInteractor _productsReadOnlyUseCases;
        private readonly IProductRentalHistoryUseCasesReadOnlyInteractor _productsRentalHistories;

        public CheckProductAvailability(IProductUseCasesReadOnlyInteractor productsReadOnlyUseCases, IProductRentalHistoryUseCasesReadOnlyInteractor productsRentalHistories)
        {
            _productsReadOnlyUseCases = productsReadOnlyUseCases;
            _productsRentalHistories = productsRentalHistories;
        }


        public override async Task<UseCaseResult<CheckProductAvailabilityResult>> Execute(Guid productId)
        {
            var productToCheckAvailability = await _productsReadOnlyUseCases.GetById(productId);
            if (!productToCheckAvailability.Success) return UseCasesResponses.ExecutionFailure<CheckProductAvailabilityResult>(productToCheckAvailability.Message);

            if (productToCheckAvailability.Result.IsAvailable)
            {
                return UseCasesResponses.SuccessfullyExecuted(new CheckProductAvailabilityResult(productToCheckAvailability.Result.IsAvailable));
            }

            var lastProductRent = await _productsRentalHistories.GetLastProductRent(productId);
            
            return !lastProductRent.Success
                ? UseCasesResponses.ExecutionFailure<CheckProductAvailabilityResult>(lastProductRent.Message)
                : UseCasesResponses.SuccessfullyExecuted(new CheckProductAvailabilityResult(lastProductRent.Result.LastProductRent.RentPeriod.EndDate));
        }
    }
}