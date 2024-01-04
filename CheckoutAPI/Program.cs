using CheckoutAPI.Controllers;
using CheckoutAPI.Services;

namespace CheckoutAPI
{
    public partial class Program
    {
        public static Dictionary<string, WatchItem> ConfigureWatchCatalog()
        {
            return new Dictionary<string, WatchItem>()
            {
                { "1", new WatchItem { Name = "Rolex", UnitPrice = 100, Discount = (3, 200) } },
                { "2", new WatchItem { Name = "Michael Kors", UnitPrice = 80, Discount = (2, 120) } },
                { "3", new WatchItem { Name = "Swatch", UnitPrice = 50, Discount = (0, 0) } },
                { "4", new WatchItem { Name = "Casio", UnitPrice = 30, Discount = (0, 0) } }
            };
        }
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Instantiate and configure watchCatalog
            Dictionary<string, WatchItem> watchCatalog = ConfigureWatchCatalog();


            // Register the watchCatalog as a singleton
            builder.Services.AddSingleton(watchCatalog);

            // Register the CheckoutService with its dependencies
            builder.Services.AddScoped<ICheckoutService, CheckoutService>();

            // Register the CheckoutController
            builder.Services.AddScoped<CheckoutController>();
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}