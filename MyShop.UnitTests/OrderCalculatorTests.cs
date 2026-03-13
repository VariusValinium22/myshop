using MyShop.MyHelpers;
using Xunit;

namespace MyShop.UnitTests
{
    public class OrderCalculatorTests
    {
        [Fact]
        public void CalculateTotal_ReturnsPriceTimesQuantity()
        {
            // Arrange
            decimal price = 9.99m;
            int quantity = 3;

            // Act
            decimal total = OrderCalculator.CalculateTotal(price, quantity);

            // Assert
            Assert.Equal(29.97m, total);
        }

        [Fact]
        public void CalculateTotal_Throws_WhenPriceIsNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                OrderCalculator.CalculateTotal(-1m, 1));
        }

        [Fact]
        public void CalculateTotal_Throws_WhenQuantityIsNegative()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                OrderCalculator.CalculateTotal(1m, -1));
        }
    }
}

// NOTE: These tests validate the xUnit test project configuration and demonstrate
// the standard Arrange / Act / Assert pattern for pure business logic.
// This file also serves as a possible reference template for future unit tests.
