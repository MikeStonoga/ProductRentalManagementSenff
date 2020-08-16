using System;
using System.Linq;
using System.Threading.Tasks;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;
using PRM.UseCases.Products.GetRentalHistories;
using PRM.UseCases.Rents;

namespace PRM.UseCases.Products.GetLastProductRents
{
    public interface IGetLastProductRent : IBaseUseCase<Guid, GetLastProductRentResult>
    {
    }
    
    public class GetLastProductRent : BaseUseCase<Guid, GetLastProductRentResult>,  IGetLastProductRent
    {
        private readonly IRentUseCasesReadOnlyInteractor _rentsUsesCases;
        private readonly IGetRentalHistory _getRentalHistory;

        public GetLastProductRent(IRentUseCasesReadOnlyInteractor rentsUsesCases, IGetRentalHistory getRentalHistory)
        {
            _rentsUsesCases = rentsUsesCases;
            _getRentalHistory = getRentalHistory;
        }

        public override async Task<UseCaseResult<GetLastProductRentResult>> Execute(Guid productId)
        {
            var rentalHistory = await _getRentalHistory.Execute(productId);
            if (!rentalHistory.Success) return UseCasesResponses.ExecutionFailure<GetLastProductRentResult>(rentalHistory.Message);

            var orderedByMostRecent = rentalHistory.Result.Items.OrderByDescending(history => history.CreationTime).ToList();
            var lastProductRentId = orderedByMostRecent[0].RentId;

            var lastProductRent = await _rentsUsesCases.GetById(lastProductRentId);
            
            return !lastProductRent.Success 
                ? UseCasesResponses.ExecutionFailure<GetLastProductRentResult>(lastProductRent.Message) 
                : UseCasesResponses.SuccessfullyExecuted(new GetLastProductRentResult(lastProductRent.Result));
        }
    }
}