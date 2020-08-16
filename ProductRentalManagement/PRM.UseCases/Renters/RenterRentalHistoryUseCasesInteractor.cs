using System;
using System.Threading.Tasks;
using PRM.Domain.Renters;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore.Dtos;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;

namespace PRM.UseCases.Renters
{
    public interface IRenterRentalHistoryUseCasesReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<RenterRentalHistory>
    {
        Task<UseCaseResult<GetAllResponse<RenterRentalHistory>>> GetRentalHistory(Guid renterId);
    }
        
    public class RenterRentalHistoryUseCasesReadOnlyInteractor : BaseUseCaseReadOnlyInteractor<RenterRentalHistory>, IRenterRentalHistoryUseCasesReadOnlyInteractor
    {
        
        public RenterRentalHistoryUseCasesReadOnlyInteractor(IReadOnlyPersistenceGateway<RenterRentalHistory> readOnlyPersistenceGateway) : base(readOnlyPersistenceGateway)
        {
        }

        public async Task<UseCaseResult<GetAllResponse<RenterRentalHistory>>> GetRentalHistory(Guid renterId)
        {
            var getRentalHistory = await ReadOnlyPersistenceGateway.GetAll(history => history.RenterId == renterId);
            return UseCasesResponses.GetUseCaseResult(getRentalHistory);
        }
    }
}