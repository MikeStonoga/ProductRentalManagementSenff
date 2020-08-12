using System.Threading.Tasks;
using PRM.Domain.Products;
using PRM.Domain.Products.Rents.Dtos;
using PRM.InterfaceAdapters.Gateways.Persistence.BaseCore;
using PRM.UseCases.BaseCore;
using PRM.UseCases.BaseCore.Extensions;

namespace PRM.UseCases.Products.GetProductRentPrice
{
    public interface IGetProductRentPrice : IBaseUseCase<GetProductRentPriceRequirement, decimal>
    {
    }
    
    public class GetProductRentPrice : BaseUseCase<GetProductRentPriceRequirement, decimal>, IGetProductRentPrice
    {
        private readonly IReadOnlyPersistenceGateway<Product> _productReadOnlyPersistenceGateway;

        public GetProductRentPrice(IReadOnlyPersistenceGateway<Product> productReadOnlyPersistenceGateway)
        {
            _productReadOnlyPersistenceGateway = productReadOnlyPersistenceGateway;
        }

        public override async Task<UseCaseResult<decimal>> Execute(GetProductRentPriceRequirement requirement)
        {
            var productToGetRentPrice = await _productReadOnlyPersistenceGateway.GetById(requirement.ProductId);
            if (!productToGetRentPrice.Success) return UseCasesResponses.PersistenceErrorResponse(0M, "ProductNotFound");
            
            
            return new UseCaseResult<decimal>
            {
                Success = true,
                Result = productToGetRentPrice.Response.CalculateProductRentPrice(requirement)
            };
        }
    }

    public class GetProductRentPriceRequirement : RentRequirement
    {
    }
}