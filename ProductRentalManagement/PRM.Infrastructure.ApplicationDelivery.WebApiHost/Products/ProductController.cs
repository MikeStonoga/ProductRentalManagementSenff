﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PRM.Domain.Products;
using PRM.Infrastructure.ApplicationDelivery.WebApiHost.BaseCore;
using PRM.InterfaceAdapters.Controllers.BaseCore;
using PRM.InterfaceAdapters.Controllers.Products;
using PRM.InterfaceAdapters.Controllers.Products.Dtos;
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

        [HttpPost]
        public async Task<ApiResponse<decimal>> GetProductRentPrice([FromBody] GetProductRentPriceInput input)
        {
            return await ManipulationController.GetProductRentPrice(input);
        }

        public async Task<ApiResponse<RentProductOutput>> RentProduct(RentProductInput input)
        {
            return await ManipulationController.RentProduct(input);
        }
    }
}