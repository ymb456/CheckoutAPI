using CheckoutAPI.Controllers;

namespace CheckoutAPI.Services
{
    public class CalculationException : Exception
    {
        public CalculationException(string message) : base(message)
        {
        }
    }
    public class CheckoutService : ICheckoutService
    {
        private readonly Dictionary<string, WatchItem> watchCatalog;

        public CheckoutService(Dictionary<string, WatchItem> watchCatalog)
        {
            this.watchCatalog = watchCatalog;
        }

        public decimal CalculateTotalPrice(List<string> watchIds)
        {

            // Creates a dictionary to store the count of each unique watch item present in the provided list of watch IDs.
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
                        throw new CalculationException($"Watch with ID {watchId} is not found in the catalog.");
                    }
                }
                else
                {
                    throw new CalculationException($"Invalid Watch ID: {watchId}");
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

            return totalPrice;
        }
    }
}
