﻿using System;
using System.Threading.Tasks;
using PRM.Domain.Renters;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore.Dtos;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;
using PRM.UseCases.Renters.GetLastRenterRents;
using PRM.UseCases.Renters.GetRentalHistories;

namespace PRM.UseCases.Renters
{
    public interface IRenterRentalHistoryUseCasesReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<RenterRentalHistory>
    {
        Task<UseCaseResult<GetAllResponse<RenterRentalHistory>>> GetRentalHistory(Guid renterId);
        Task<UseCaseResult<GetLastRenterRentResult>> GetLastRenterRent(Guid productId);
    }
        
    public class RenterRentalHistoryUseCasesReadOnlyInteractor : BaseUseCaseReadOnlyInteractor<RenterRentalHistory>, IRenterRentalHistoryUseCasesReadOnlyInteractor
    {
        private readonly IGetRenterRentalHistory _getRenterRentalHistory;
        private readonly IGetLastRenterRent _getLastRenterRent;
        public RenterRentalHistoryUseCasesReadOnlyInteractor(IReadOnlyPersistenceGateway<RenterRentalHistory> readOnlyPersistenceGateway, IGetRenterRentalHistory getRenterRentalHistory, IGetLastRenterRent getLastRenterRent) : base(readOnlyPersistenceGateway)
        {
            _getRenterRentalHistory = getRenterRentalHistory;
            _getLastRenterRent = getLastRenterRent;
        }

        public async Task<UseCaseResult<GetAllResponse<RenterRentalHistory>>> GetRentalHistory(Guid renterId)
        {
            return await UseCasesResponses.GetUseCaseExecutionResponse<IGetRenterRentalHistory, Guid, GetAllResponse<RenterRentalHistory>>(_getRenterRentalHistory, renterId);
        }

        public async Task<UseCaseResult<GetLastRenterRentResult>> GetLastRenterRent(Guid renterId)
        {
            return await UseCasesResponses.GetUseCaseExecutionResponse<IGetLastRenterRent, Guid, GetLastRenterRentResult>(_getLastRenterRent, renterId);
        }
    }
}