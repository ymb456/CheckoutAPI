namespace CheckoutAPI.Services
{
    public interface ICheckoutService
    {
        decimal CalculateTotalPrice(List<string> watchIds);
    }
}
