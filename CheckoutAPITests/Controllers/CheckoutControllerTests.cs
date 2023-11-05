using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CheckoutAPI.Controllers;

namespace QuickBuyAPI.Controllers.Tests
{
    [TestClass()]
    public class CheckoutControllerTests
    {
        [TestMethod()]
        public void CheckoutTest_ReturnsCorrectTotalPrice()
        {
            // Arrange
            var logger = NullLogger<CheckoutController>.Instance;
            var controller = new CheckoutController(logger);
            var watchIds = new List<string> { "001", "002", "001", "004", "003" };

            // Act
            var result = controller.Checkout(watchIds) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            var price = (decimal)result.Value.GetType().GetProperty("price").GetValue(result.Value, null);
            Assert.AreEqual(360m, price, "Total price should be 360");
        }
        [TestMethod()]
        public void CheckoutTest_EmptyWatchIds_ReturnsZeroPrice()
        {
            // Arrange
            var logger = NullLogger<CheckoutController>.Instance;
            var controller = new CheckoutController(logger);
            var watchIds = new List<string>();

            // Act
            var result = controller.Checkout(watchIds) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            var price = (decimal)result.Value.GetType().GetProperty("price").GetValue(result.Value, null);
            Assert.AreEqual(0m, price, "Total price should be 0");
        }

        [TestMethod()]
        public void CheckoutTest_SingleWatch_ReturnsCorrectPrice()
        {
            // Arrange
            var logger = NullLogger<CheckoutController>.Instance;
            var controller = new CheckoutController(logger);
            var watchIds = new List<string> { "001" };

            // Act
            var result = controller.Checkout(watchIds) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            var price = (decimal)result.Value.GetType().GetProperty("price").GetValue(result.Value, null);
            Assert.AreEqual(100m, price, "Total price should be 100");
        }

        [TestMethod()]
        public void CheckoutTest_InvalidWatchIds_ReturnsBadRequest()
        {
            // Arrange
            var logger = NullLogger<CheckoutController>.Instance;
            var controller = new CheckoutController(logger);
            var watchIds = new List<string> { "005" };

            // Act
            var result = controller.Checkout(watchIds) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Watch with ID 005 is not found in the catalog.", result.Value);
        }

        [TestMethod()]
        public void CheckoutTest_DiscountApplied_ReturnsCorrectTotalPrice()
        {
            // Arrange
            var logger = NullLogger<CheckoutController>.Instance;
            var controller = new CheckoutController(logger);
            var watchIds = new List<string> { "001", "001", "001" }; // Three Rolexes

            // Act
            var result = controller.Checkout(watchIds) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            var price = (decimal)result.Value.GetType().GetProperty("price").GetValue(result.Value, null);
            Assert.AreEqual(200m, price, "Total price should be 200");
        }

        [TestMethod()]
        public void CheckoutTest_NoDiscountApplied_ReturnsCorrectTotalPrice()
        {
            // Arrange
            var logger = NullLogger<CheckoutController>.Instance;
            var controller = new CheckoutController(logger);
            var watchIds = new List<string> { "003", "004" }; // A Swatch and a Casio

            // Act
            var result = controller.Checkout(watchIds) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            var price = (decimal)result.Value.GetType().GetProperty("price").GetValue(result.Value, null);
            Assert.AreEqual(80m, price, "Total price should be 80");
        }

        [TestMethod()]
        public void CheckoutTest_InvalidWatchId_ReturnsBadRequest()
        {
            // Arrange
            var logger = NullLogger<CheckoutController>.Instance;
            var controller = new CheckoutController(logger);
            var watchIds = new List<string> { "005" }; // Invalid watchId

            // Act
            var result = controller.Checkout(watchIds) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Watch with ID 005 is not found in the catalog.", result.Value);
        }

    }
}
