using System;
using System.Linq;
using System.Threading.Tasks;
using PRM.Domain.Rents;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;
using PRM.UseCases.Renters.GetRentalHistories;
using PRM.UseCases.Rents;

namespace PRM.UseCases.Renters.GetLastRenterRents
{
    public interface IGetLastRenterRent : IBaseUseCase<Guid, GetLastRenterRentResult>
    {
    }
    
    public class GetLastRenterRent : BaseUseCase<Guid, GetLastRenterRentResult>,  IGetLastRenterRent
    {
        private readonly IRentUseCasesReadOnlyInteractor _rentsUsesCases;
        private readonly IGetRenterRentalHistory _getRenterRentalHistory;

        public GetLastRenterRent(IRentUseCasesReadOnlyInteractor rentsUsesCases, IGetRenterRentalHistory getRenterRentalHistory)
        {
            _rentsUsesCases = rentsUsesCases;
            _getRenterRentalHistory = getRenterRentalHistory;
        }

        public override async Task<UseCaseResult<GetLastRenterRentResult>> Execute(Guid productId)
        {
            var rentalHistory = await _getRenterRentalHistory.Execute(productId);
            if (!rentalHistory.Success) return UseCasesResponses.ExecutionFailure<GetLastRenterRentResult>(rentalHistory.Message);
            if (rentalHistory.Result.TotalCount == 0) return UseCasesResponses.SuccessfullyExecuted<GetLastRenterRentResult>("Dont have any rent yet");

            var orderedByMostRecent = rentalHistory.Result.Items.OrderByDescending(history => history.CreationTime).ToList();
            var lastRenterRentId = orderedByMostRecent[0].RentId;

            var lastRenterRent = await _rentsUsesCases.GetById(lastRenterRentId);
            
            return !lastRenterRent.Success 
                ? UseCasesResponses.ExecutionFailure<GetLastRenterRentResult>(lastRenterRent.Message) 
                : UseCasesResponses.SuccessfullyExecuted(new GetLastRenterRentResult(lastRenterRent.Result));
        }
    }
}