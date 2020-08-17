using System;
using System.Collections.Generic;
using PRM.Domain.BaseCore.ValueObjects;
using PRM.Domain.Products;
using PRM.Domain.Products.Enums;
using PRM.Domain.Renters;
using PRM.Domain.Rents;
using PRM.Domain.Rents.Enums;
using Xunit;

namespace PRM.Tests.Rents.Domain.Entities
{
    public class RentTests
    {
        [Fact]
        public void CreateRentSuccessfuly()
        {
            // Arrange
            var rentPeriod = new DateRange(DateTime.Now, DateTime.Now.AddDays(10));
            var productsToRent = GetProductsToRent();
            var renter = new Renter();
            
            // Act
            var rent = new Rent(rentPeriod, productsToRent, renter);
            
            // Assert
            Assert.Equal(renter.Id, rent.RenterId);
            Assert.Equal(RentStatus.Open, rent.Status);
            Assert.Equal(rentPeriod, rent.RentPeriod);
            Assert.Equal(30, rent.DailyPrice);
            Assert.Equal(15, rent.DailyLateFee);
            Assert.False(rent.WasProductDamaged);
            Assert.Equal(0, rent.DamageFee);
            Assert.Equal(0, rent.Discount);
            Assert.Equal(300, rent.CurrentRentPaymentValue);
            Assert.Equal(300, rent.PriceWithoutFees);
            Assert.Equal(10, rent.RentDays);
            Assert.Equal(0, rent.LateFee);
            Assert.False(rent.IsLate);
            Assert.Equal(-10, rent.LateDays);
            Assert.False(rent.IsFinished);
        }

        private List<Product> GetProductsToRent()
        {
            var product1 = new Product
            {
                Status = ProductStatus.Available,
                RentDailyPrice = 10,
                RentDailyLateFee = 5
            };
            
            var product2 = new Product
            {
                Status = ProductStatus.Available,
                RentDailyPrice = 20,
                RentDailyLateFee = 10
            };

            var productsToRent = new List<Product>
            {
                product1, 
                product2
            };

            return productsToRent;
        }
    }
}