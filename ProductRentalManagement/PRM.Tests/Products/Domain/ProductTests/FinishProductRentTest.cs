/*using System;
using System.Collections.Generic;
using PRM.Domain.Products;
using PRM.Domain.Products.Rents;
using PRM.Domain.Products.Rents.Enums;
using PRM.Domain.Rents;
using Xunit;

namespace PRM.Tests.Products.Domain.ProductTests
{
    public interface IFinishProductRentTest
    {
        public void CanFinishProductRent();
        public void CannotFinishProductRent();
    }
    
    public class FinishProductRentTest : IFinishProductRentTest
    {
        [Fact] 
        public void CanFinishProductRent()
        {
            // Arrange
            var rentId = Guid.Parse("ed7d0b83-2339-4c31-b7dd-15e966c7382a");
            var productId = Guid.Parse("125aaa00-7851-4ce4-b96a-93773fc2b481");
            
            var product = new Product
            {
                Id = productId,
                Code = "1",
                Name = "Product1",
                Description = "ModelA",
                ProductRentId = new List<Rent>
                {
                    new Rent
                    {
                        Id = rentId,
                        StartDate = DateTime.Now.AddDays(-31),
                        EndDate = DateTime.Now,
                        DailyPrice = 0.5M,
                        DailyLateFee = 0.1M,
                        ProductId = productId
                    }
                },
                Status = RentStatus.Available,
                CreationTime = DateTime.Now,
                CreatorId = Guid.Parse("8b373509-c0af-4043-a6b9-cca7c3fdb7ae")
            };

            var finishRentRequirement = new FinishRentRequirement
            {
                ProductRentId = rentId,
                DamageFee = 0
            };
            // Act
            var finishProductRentResponse = product.FinishProductRent(finishRentRequirement);

            // Assert
            Assert.True(finishProductRentResponse.Success);
            Assert.Equal("RentFinished", finishProductRentResponse.Message);
            Assert.Equal(15.5M, finishProductRentResponse.Result.ValueToPay);
        }
        
        [Fact] 
        public void CannotFinishProductRent()
        {
            // Arrange
            var rentId = Guid.Parse("ed7d0b83-2339-4c31-b7dd-15e966c7382a");
            var productId = Guid.Parse("125aaa00-7851-4ce4-b96a-93773fc2b481");
            
            var product = new Product
            {
                Id = productId,
                Code = "1",
                Name = "Product1",
                Description = "ModelA",
                ProductRentId = new List<Rent>
                {
                    new Rent
                    {
                        Id = Guid.Parse("fc46327f-6233-48c1-b511-3289fbbf00e5"),
                        StartDate = DateTime.Now.AddDays(-31),
                        EndDate = DateTime.Now,
                        DailyPrice = 0.5M,
                        DailyLateFee = 0.1M,
                        ProductId = productId
                    }
                },
                Status = RentStatus.Available,
                CreationTime = DateTime.Now,
                CreatorId = Guid.Parse("8b373509-c0af-4043-a6b9-cca7c3fdb7ae")
            };
            
            var finishRentRequirement = new FinishRentRequirement
            {
                ProductRentId = rentId,
                DamageFee = 0
            };
            
            // Act
            var finishProductRentResponse = product.FinishProductRent(finishRentRequirement);

            // Assert
            Assert.False(finishProductRentResponse.Success);
            Assert.Equal("RentNotFound", finishProductRentResponse.Message);
            Assert.Equal(0, finishProductRentResponse.Result.ValueToPay);
        }
    }
}*/