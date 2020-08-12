using System;
using PRM.Domain.Products;
using PRM.Domain.Products.Rents.Dtos;
using Xunit;

namespace PRM.Tests.Products.Domain.ProductTests
{
    public interface ICalculateProductRentPriceTest
    {
        public void CanCalculateProductRentPrice();
    }
    
    public class CalculateProductRentPriceTest : ICalculateProductRentPriceTest
    {
        [Fact]
        public void CanCalculateProductRentPrice()
        {
            // Arrange
            
            var rentRequirement = new RentRequirement
            {
                DailyPrice = 0.5M,
                StartDate = new DateTime(2020, 01, 01),
                EndDate = new DateTime(2020, 01, 11),
            };
            
            var product = new Product();

            // Act
            var productRentPrice = product.CalculateProductRentPrice(rentRequirement);

            // Assert
            Assert.Equal(5.0M, productRentPrice);
        }
    }
}