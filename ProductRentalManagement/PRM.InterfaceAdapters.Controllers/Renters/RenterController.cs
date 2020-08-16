using System;
using System.Threading.Tasks;
using PRM.Domain.BaseCore.ValueObjects;
using PRM.Domain.Renters;
using PRM.InterfaceAdapters.Controllers.BaseCore;
using PRM.InterfaceAdapters.Controllers.BaseCore.Extensions;
using PRM.InterfaceAdapters.Controllers.Renters.Dtos;
using PRM.InterfaceAdapters.Controllers.Renters.Dtos.GetBirthDaysOnPeriods;
using PRM.InterfaceAdapters.Controllers.Renters.Dtos.RentalHistory;
using PRM.UseCases.Renters;

namespace PRM.InterfaceAdapters.Controllers.Renters
{
    public interface IRenterReadOnlyController : IBaseReadOnlyController<Renter, RenterOutput>
    {
        Task<ApiResponse<GetAllResponse<Renter, RenterOutput>>> GetBirthDaysOnPeriod(GetBirthDaysOnPeriodInput input);
        Task<ApiResponse<GetAllResponse<RenterRentalHistory, RenterRentalHistoryOutput>>> GetRentalHistory(Guid renterId);
    }
     
    public class RenterReadOnlyController : BaseReadOnlyController<Renter, RenterOutput, IRenterUseCasesReadOnlyInteractor>, IRenterReadOnlyController
    {
        private readonly IRenterRentalHistoryUseCasesReadOnlyInteractor _renterRentalHistoryUseCasesReadOnlyInteractor;
        public RenterReadOnlyController(IRenterUseCasesReadOnlyInteractor useCaseReadOnlyInteractor, IRenterRentalHistoryUseCasesReadOnlyInteractor renterRentalHistoryUseCasesReadOnlyInteractor) : base(useCaseReadOnlyInteractor)
        {
            _renterRentalHistoryUseCasesReadOnlyInteractor = renterRentalHistoryUseCasesReadOnlyInteractor;
        }

        public async Task<ApiResponse<GetAllResponse<Renter, RenterOutput>>> GetBirthDaysOnPeriod(GetBirthDaysOnPeriodInput input)
        {
            var period = DateRangeProvider.GetDateRange(input.StartDate, input.EndDate);
            if (!period.Success) return ApiResponses.FailureResponse<GetAllResponse<Renter, RenterOutput>>(period.Message);

            return await GetAll(r => period.Result.IsMonthOnRange(r.BirthDate.Date.Month));
        }

        public async Task<ApiResponse<GetAllResponse<RenterRentalHistory, RenterRentalHistoryOutput>>> GetRentalHistory(Guid renterId)
        {
            return await ApiResponses.GetUseCaseInteractorResponse<RenterRentalHistory, RenterRentalHistoryOutput>(_renterRentalHistoryUseCasesReadOnlyInteractor.GetRentalHistory, renterId);
        }
    }

    public interface IRenterManipulationController : IBaseManipulationController<Renter, RenterInput, RenterOutput>, IRenterReadOnlyController
    {

    }

    public class RenterManipulationController : BaseManipulationController<Renter, RenterInput, RenterOutput, IRenterUseCasesManipulationInteractor, IRenterReadOnlyController>, IRenterManipulationController
    {
        
        public RenterManipulationController(IRenterUseCasesManipulationInteractor useCaseInteractor, IRenterReadOnlyController readOnlyController, IRenterRentalHistoryUseCasesReadOnlyInteractor renterRentalHistoryUseCasesReadOnlyInteractor) : base(useCaseInteractor, readOnlyController)
        {
        }

        public async Task<ApiResponse<GetAllResponse<Renter, RenterOutput>>> GetBirthDaysOnPeriod(GetBirthDaysOnPeriodInput input)
        {
            return await ReadOnlyController.GetBirthDaysOnPeriod(input);
        }

        public async Task<ApiResponse<GetAllResponse<RenterRentalHistory, RenterRentalHistoryOutput>>> GetRentalHistory(Guid renterId)
        {
            return await ReadOnlyController.GetRentalHistory(renterId);
        }
    }
}