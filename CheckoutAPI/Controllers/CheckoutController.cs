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
        private Dictionary<string, WatchItem> watchCatalog = new Dictionary<string, WatchItem>()
        {
            { "1", new WatchItem { Name = "Rolex", UnitPrice = 100, Discount = (3, 200) } },
            { "2", new WatchItem { Name = "Michael Kors", UnitPrice = 80, Discount = (2, 120) } },
            { "3", new WatchItem { Name = "Swatch", UnitPrice = 50, Discount = (0, 0) } }, 
            { "4", new WatchItem { Name = "Casio", UnitPrice = 30, Discount = (0, 0) } }  
        };

        public CheckoutController(ILogger<CheckoutController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "checkout")]
        public IActionResult Checkout([FromBody] List<string> watchIds)
        {
            if (watchIds.Count > 10)
            {
                // Creates a dictionary to store the count of each unique watch item present in the provided list of watch IDs.
                return BadRequest("Exceeded the maximum allowed number of items in the request.");
            }

            var watchCounts = new Dictionary<string, int>();
            foreach (var watchId in watchIds)
            {
                if (int.TryParse(watchId, out int parsedWatchId))
                {
                    string parsedId = parsedWatchId.ToString();
                    if (watchCatalog.ContainsKey(parsedId))
                    {
                        if (watchCounts.ContainsKey(parsedId))
                        {
                            watchCounts[parsedId]++;
                        }
                        else
                        {
                            watchCounts[parsedId] = 1;
                        }
                    }
                    else
                    {
                        return BadRequest($"Watch with ID {parsedId} is not found in the catalog.");
                    }
                }
                else
                {
                    return BadRequest($"Invalid Watch ID: {watchId}");
                }
            }

            decimal totalPrice = 0;
            foreach (var watchId in watchCounts.Keys)
            {
                var watch = watchCatalog[watchId];
                var count = watchCounts[watchId];

                if (watch.Discount.Count != 0 && count >= watch.Discount.Count)
                {
                    // Calculates the total price by applying the appropriate discounts based on the count of the current watch item.
                    // The formula consists of two parts: the total price after applying the discounted price for the given
                    // quantity and the remaining price for the remaining quantity, if any.
                    totalPrice += (count / watch.Discount.Count) * watch.Discount.Discount + (count % watch.Discount.Count) * watch.UnitPrice;
                }
                else
                {
                    totalPrice += count * watch.UnitPrice;
                }
            }

            var response = new { price = totalPrice };
            return Ok(response);
        }
    }
}
