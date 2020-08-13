using System.Threading.Tasks;
using PRM.Domain.Products;
using PRM.InterfaceAdapters.Controllers.BaseCore;
using PRM.InterfaceAdapters.Controllers.BaseCore.Extensions;
using PRM.InterfaceAdapters.Controllers.Products.Dtos;
using PRM.InterfaceAdapters.Controllers.Products.Dtos.GetProductRentPriceDtos;
using PRM.InterfaceAdapters.Controllers.Products.Dtos.RentProductDtos;
using PRM.UseCases.Products;

namespace PRM.InterfaceAdapters.Controllers.Products
{
    public interface IProductReadOnlyController : IBaseReadOnlyController<Product, ProductOutput>
    {
        Task<ApiResponse<decimal>> GetProductRentPrice(GetProductRentPriceInput input);
    }
     
    public class ProductReadOnlyController : BaseReadOnlyController<Product, ProductOutput, IProductUseCasesReadOnlyInteractor>, IProductReadOnlyController
    {
        public ProductReadOnlyController(IProductUseCasesReadOnlyInteractor useCaseReadOnlyInteractor) : base(useCaseReadOnlyInteractor)
        {
        }

        public async Task<ApiResponse<decimal>> GetProductRentPrice(GetProductRentPriceInput input)
        {
            var useCaseResult = await UseCaseReadOnlyInteractor.GetProductRentPrice(input);
            return useCaseResult.Success
                ? ApiResponses.SuccessfullyExecutedResponse(useCaseResult.Result)
                : ApiResponses.FailureResponse(useCaseResult.Result);
        }
    }

    public interface IProductManipulationController : IBaseManipulationController<Product, ProductInput, ProductOutput>, IProductReadOnlyController
    {
        Task<ApiResponse<RentProductOutput>> RentProduct(RentProductInput input);

    }

    public class ProductManipulationController : BaseManipulationController<Product, ProductInput, ProductOutput, IProductUseCasesManipulationInteractor, IProductReadOnlyController>, IProductManipulationController
    {
        public ProductManipulationController(IProductUseCasesManipulationInteractor useCaseInteractor, IProductReadOnlyController readOnlyController) : base(useCaseInteractor, readOnlyController)
        {
        }
        
        
        
        

        public async Task<ApiResponse<decimal>> GetProductRentPrice(GetProductRentPriceInput input)
        {
            return await ReadOnlyController.GetProductRentPrice(input);
        }

        public async Task<ApiResponse<RentProductOutput>> RentProduct(RentProductInput input)
        {
            var rentProductResponse = await UseCaseInteractor.RentProduct(input);
            var output = new RentProductOutput(rentProductResponse.Result);
            
            return rentProductResponse.Success
                ? ApiResponses.SuccessfullyExecutedResponse(output, rentProductResponse.Message)
                : ApiResponses.FailureResponse(output, rentProductResponse.Message);
        }
    }
}