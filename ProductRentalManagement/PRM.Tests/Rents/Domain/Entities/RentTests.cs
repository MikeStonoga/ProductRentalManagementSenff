using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PRM.Domain.BaseCore.ValueObjects;
using PRM.Domain.Products;
using PRM.Domain.Products.Enums;
using PRM.Domain.Renters;
using PRM.Domain.Rents;
using Xunit;

namespace PRM.Tests.Rents.Domain.Entities
{
    public interface IRentTests
    {
        void ItCreateNotLateRentSuccessfully();
        void ItCreateLateRentSuccessfully();
        void ItFailsToCreateWithoutProducts();

    }

    public class RentTests : IRentTests
    {
        [Fact]
        public void ItCreateNotLateRentSuccessfully()
        {
            // Arrange
            var rentPeriod = new DateRange(DateTime.Now, DateTime.Now.AddDays(10));
            var productsToRent = GetProductsToRent();
            var renter = new Renter();

            // Act
            var rent = new Rent(rentPeriod, productsToRent, renter);

            // Assert
            Assert.Equal(renter.Id, rent.RenterId);
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
            Assert.True(rent.IsOpen);
            Assert.False(rent.IsLate);
            Assert.Equal(0, rent.LateDays);
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

        [Fact]
        public void ItCreateLateRentSuccessfully()
        {
            // Arrange
            var rentPeriod = new DateRange(DateTime.Now.Date.AddDays(-11), DateTime.Now.Date.AddDays(-1));
            var productsToRent = GetProductsToRent();
            var renter = new Renter();

            // Act
            var rent = new Rent(rentPeriod, productsToRent, renter);

            // Assert
            Assert.Equal(renter.Id, rent.RenterId);
            Assert.Equal(rentPeriod, rent.RentPeriod);
            Assert.Equal(30, rent.DailyPrice);
            Assert.Equal(15, rent.DailyLateFee);
            Assert.False(rent.WasProductDamaged);
            Assert.Equal(0, rent.DamageFee);
            Assert.Equal(0, rent.Discount);
            Assert.Equal(315, rent.CurrentRentPaymentValue);
            Assert.Equal(300, rent.PriceWithoutFees);
            Assert.Equal(11, rent.RentDays);
            Assert.Equal(15, rent.LateFee);
            Assert.True(rent.IsLate);
            Assert.Equal(1, rent.LateDays);
            Assert.True(rent.IsOpen);
            Assert.False(rent.IsClosed);
            Assert.False(rent.IsFinished);
        }

        [Fact]
        public void ItFailsToCreateWithoutProducts()
        {
            // Arrange
            var rentPeriod = new DateRange(DateTime.Now.Date.AddDays(-11), DateTime.Now.Date.AddDays(-1));
            var productsToRent = new List<Product>();
            var renter = new Renter();

            // Act
            var exception = Assert.Throws<ValidationException>(() => new Rent(rentPeriod, productsToRent, renter));

            // Assert
            Assert.Equal("Trying to create a Rent without any Products", exception.Message);
        }
    }
}