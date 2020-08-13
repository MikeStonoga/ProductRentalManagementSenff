using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PRM.Domain.Products;
using PRM.Domain.Products.Rents.Dtos;
using PRM.InterfaceAdapters.Controllers.BaseCore;
using PRM.InterfaceAdapters.Controllers.BaseCore.Extensions;
using PRM.InterfaceAdapters.Controllers.Products.Dtos;
using PRM.InterfaceAdapters.Controllers.Products.Dtos.FinishRentDtos;
using PRM.InterfaceAdapters.Controllers.Products.Dtos.GetProductRentPriceDtos;
using PRM.InterfaceAdapters.Controllers.Products.Dtos.RentProductDtos;
using PRM.UseCases.BaseCore;
using PRM.UseCases.Products;

namespace PRM.InterfaceAdapters.Controllers.Products
{
    public interface IProductReadOnlyController : IBaseReadOnlyController<Product, ProductOutput>
    {
        Task<ApiResponse<decimal>> GetProductRentPrice(GetProductRentPriceInput input);
        Task<ApiResponse<GetAllResponse<Product, ProductOutput>>> GetAllWithRents();
    }
     
    public class ProductReadOnlyController : BaseReadOnlyController<Product, ProductOutput, IProductUseCasesReadOnlyInteractor>, IProductReadOnlyController
    {
        public ProductReadOnlyController(IProductUseCasesReadOnlyInteractor useCaseReadOnlyInteractor) : base(useCaseReadOnlyInteractor)
        {
        }
        
        public async Task<ApiResponse<GetAllResponse<Product, ProductOutput>>> GetAllWithRents()
        {
            return await GetAll(p => p.Rents);
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
        Task<ApiResponse<FinishRentOutput>> FinishRent(FinishRentInput input);

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

        public async Task<ApiResponse<GetAllResponse<Product, ProductOutput>>> GetAllWithRents()
        {
            return await ReadOnlyController.GetAllWithRents();
        }

        public async Task<ApiResponse<RentProductOutput>> RentProduct(RentProductInput input)
        {
            return await GetUseCaseExecutionResponse<RentRequirement, RentResult, RentProductInput, RentProductOutput>(UseCaseInteractor.RentProduct, input);
        }

        public async Task<ApiResponse<FinishRentOutput>> FinishRent(FinishRentInput input)
        {
            return await GetUseCaseExecutionResponse<FinishRentRequirement, RentFinishedResult, FinishRentInput, FinishRentOutput>(UseCaseInteractor.FinishRent, input);
        }

        public async Task<ApiResponse<TOutput>> GetUseCaseExecutionResponse<TUseCaseRequirement, TUseCaseResult, TInput, TOutput>(Func<TUseCaseRequirement, Task<UseCaseResult<TUseCaseResult>>> useCase, TInput input)
            where TInput : TUseCaseRequirement 
            where TOutput : class, TUseCaseResult, new()
        {
            var useCaseResponse = await useCase(input);
            if (!useCaseResponse.Success) return ApiResponses.FailureResponse<TOutput>(useCaseResponse.Message);
            
            var output = Activator.CreateInstance(typeof(TOutput), useCaseResponse.Result) as TOutput;
            return ApiResponses.SuccessfullyExecutedResponse(output, useCaseResponse.Message);
        }
    }
}