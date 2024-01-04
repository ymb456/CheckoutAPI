using CheckoutAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CheckoutAPI.Controllers
{
    public class WatchItem
    {
        public string? Name { get; set; }
        public decimal UnitPrice { get; set; }
        public (int Count, decimal Discount) Discount { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly ILogger<CheckoutController> _logger;
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ILogger<CheckoutController> logger, ICheckoutService checkoutService)
        {
            _logger = logger;
            _checkoutService = checkoutService;
        }

        [HttpPost(Name = "checkout")]
        public IActionResult Checkout([FromBody] List<string> watchIds)
        {
            try
            {
                decimal totalPrice = _checkoutService.CalculateTotalPrice(watchIds);

                var response = new { price = totalPrice };
                return Ok(response);
            }
            catch (CalculationException ex)
            {
                // Handle exceptions thrown by the service
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other unexpected exceptions
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
