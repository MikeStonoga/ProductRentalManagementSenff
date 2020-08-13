using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PRM.Domain.Products;
using PRM.Infrastructure.ApplicationDelivery.WebApiHost.BaseCore;
using PRM.InterfaceAdapters.Controllers.BaseCore;
using PRM.InterfaceAdapters.Controllers.Products;
using PRM.InterfaceAdapters.Controllers.Products.Dtos;
using PRM.InterfaceAdapters.Controllers.Products.Dtos.FinishRentDtos;
using PRM.InterfaceAdapters.Controllers.Products.Dtos.GetProductRentPriceDtos;
using PRM.InterfaceAdapters.Controllers.Products.Dtos.RentProductDtos;
using PRM.UseCases.Products;

namespace PRM.Infrastructure.ApplicationDelivery.WebApiHost.Products
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : BaseManipulationWebController<Product, ProductInput, ProductOutput, IProductUseCasesManipulationInteractor, IProductManipulationController>, IProductManipulationController
    {
        public ProductController(IProductUseCasesManipulationInteractor useCaseInteractor, IProductManipulationController manipulationController) : base(useCaseInteractor, manipulationController)
        {
        }
        
        [HttpGet]
        public async Task<ApiResponse<GetAllResponse<Product, ProductOutput>>> GetAllWithRents()
        {
            return await GetAll(product => product.Rents);
        }

        [HttpPost]
        public async Task<ApiResponse<decimal>> GetProductRentPrice([FromBody] GetProductRentPriceInput input)
        {
            return await ManipulationController.GetProductRentPrice(input);
        }



        [HttpPost]
        public async Task<ApiResponse<RentProductOutput>> RentProduct(RentProductInput input)
        {
            return await ManipulationController.RentProduct(input);
        }
        
        [HttpPost]        
        public async Task<ApiResponse<FinishRentOutput>> FinishRent(FinishRentInput input)
        {
            return await ManipulationController.FinishRent(input);
        }
    }
}