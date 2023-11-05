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
    }
}
