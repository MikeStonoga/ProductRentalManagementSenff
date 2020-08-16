using System;
using System.Threading.Tasks;
using PRM.Domain.Products;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore.Dtos;
using PRM.UseCases.BaseCore;

namespace PRM.UseCases.Products
{
    public interface IProductRentalHistoryUseCasesReadOnlyInteractor : IBaseUseCaseReadOnlyInteractor<ProductRentalHistory>
    {
        Task<UseCaseResult<GetAllResponse<ProductRentalHistory>>> GetRentalHistory(Guid productId);
    }
        
    public class ProductRentalHistoryUseCasesReadOnlyInteractor : BaseUseCaseReadOnlyInteractor<ProductRentalHistory>, IProductRentalHistoryUseCasesReadOnlyInteractor
    {
        
        public ProductRentalHistoryUseCasesReadOnlyInteractor(IReadOnlyPersistenceGateway<ProductRentalHistory> readOnlyPersistenceGateway) : base(readOnlyPersistenceGateway)
        {
        }

        public async Task<UseCaseResult<GetAllResponse<ProductRentalHistory>>> GetRentalHistory(Guid productId)
        {
            var getRentalHistory = await ReadOnlyPersistenceGateway.GetAll(history => history.ProductId == productId);
            return GetUseCaseResult(getRentalHistory);
        }
    }
}