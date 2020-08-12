using System;
using System.Collections.Generic;
using PRM.Domain.Products;
using PRM.Domain.Products.Rents;
using PRM.Domain.Products.Rents.Dtos;
using PRM.Domain.Products.Rents.Enums;
using Xunit;

namespace PRM.Tests.Products.Domain.ProductTests
{
    public interface IRentProductTest
    {
        public void CanRentAvailableProduct();
        public void CanRentUnavailableProduct();
    }
    
    public class RentProductTest : IRentProductTest
    {
        [Fact]
        public void CanRentAvailableProduct()
        {
            // Arrange
            var productId = Guid.Parse("ed7d0b83-2339-4c31-b7dd-15e966c7382a"); 
            var product = new Product
            {
                Id = productId,
                Code = "1",
                Name = "Product1",
                Description = "ModelA",
                Rents = new List<Rent>(),
                Status = RentStatus.Available,
                CreationTime = DateTime.Now,
                CreatorId = Guid.Parse("8b373509-c0af-4043-a6b9-cca7c3fdb7ae")
            };

            var renterId = Guid.Parse("740316e1-7fa3-4099-b496-6c72a83f6499");
            
            var rentRequirement = new RentRequirement
            {
                DailyPrice = 0.5M,
                StartDate = new DateTime(2020, 08, 10),
                EndDate = new DateTime(2020, 08, 12),
                ProductId = productId,
                RenterId = renterId,
                DailyLateFee = 0.2M,
                Name = "Student Rent"
            };
            // Act
            var rentProductResponse = product.RentProduct(rentRequirement);
            
            // Assert
            Assert.True(rentProductResponse.Success);
            Assert.Equal("Rented", rentProductResponse.Message);
            Assert.Equal(1M, rentProductResponse.Result.CurrentRentPaymentValue);
        }
        
        [Fact]
        public void CanRentUnavailableProduct()
        {
            // Arrange
            var productId = Guid.Parse("ed7d0b83-2339-4c31-b7dd-15e966c7382a"); 
            var product = new Product
            {
                Id = productId,
                Code = "1",
                Name = "Product1",
                Description = "ModelA",
                Rents = new List<Rent>(),
                Status = RentStatus.Unavailable,
                CreationTime = DateTime.Now,
                CreatorId = Guid.Parse("8b373509-c0af-4043-a6b9-cca7c3fdb7ae")
            };

            var renterId = Guid.Parse("740316e1-7fa3-4099-b496-6c72a83f6499");
            
            var rentRequirement = new RentRequirement
            {
                DailyPrice = 0.5M,
                StartDate = new DateTime(2020, 08, 10),
                EndDate = new DateTime(2020, 08, 12),
                ProductId = productId,
                RenterId = renterId,
                DailyLateFee = 0.2M,
                Name = "Student Rent"
            };
            
            // Act
            var rentProductResponse = product.RentProduct(rentRequirement);
            
            // Assert
            Assert.False(rentProductResponse.Success);
            Assert.Equal("AlreadyRentedProduct", rentProductResponse.Message);
            Assert.Equal(0, rentProductResponse.Result.CurrentRentPaymentValue);
        }
    }
}