using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CheckoutAPI.Controllers;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using CheckoutAPI.Services;

namespace QuickBuyAPI.Controllers.Tests
{
    [TestClass()]
    public class CheckoutControllerTests
    {

        private static TestContext _testContext = null!;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod()]
        public void CheckoutTest_ReturnsCorrectTotalPrice()
        {
            // Arrange
            var logger = NullLogger<CheckoutController>.Instance;
            var service = new CheckoutService(CheckoutAPI.Program.ConfigureWatchCatalog());
            var controller = new CheckoutController(logger, service);
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
            var service = new CheckoutService(CheckoutAPI.Program.ConfigureWatchCatalog());
            var controller = new CheckoutController(logger, service);
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
            var service = new CheckoutService(CheckoutAPI.Program.ConfigureWatchCatalog());
            var controller = new CheckoutController(logger, service);
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
            var service = new CheckoutService(CheckoutAPI.Program.ConfigureWatchCatalog());
            var controller = new CheckoutController(logger, service);
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
            var service = new CheckoutService(CheckoutAPI.Program.ConfigureWatchCatalog());
            var controller = new CheckoutController(logger, service);
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
            var service = new CheckoutService(CheckoutAPI.Program.ConfigureWatchCatalog());
            var controller = new CheckoutController(logger, service);
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
            var service = new CheckoutService(CheckoutAPI.Program.ConfigureWatchCatalog());
            var controller = new CheckoutController(logger, service);
            var watchIds = new List<string> { "005" }; // Invalid watchId

            // Act
            var result = controller.Checkout(watchIds) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Watch with ID 005 is not found in the catalog.", result.Value);
        }

        [TestMethod()]
        public async Task CheckoutEndpoint_ReturnsCorrectTotalPrice()
        {
            // Arrange

            Console.WriteLine(_testContext.TestName);

            await using var factory = new WebApplicationFactory<CheckoutAPI.Program> ();
            
            using var client = factory.CreateClient();


            var watchIds = new List<string> { "001", "002", "001", "004", "003" };
            var content = new StringContent(JsonConvert.SerializeObject(watchIds), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("https://localhost:7248/checkout", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(responseContent);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(responseObject.ContainsKey("price"));
            Assert.AreEqual(360m, responseObject["price"]);
        }
    }
}
