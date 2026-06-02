namespace CarSales.Contracts.DTOs.Request.Car
{
    public class CarPageRequest : PageRequest
    {
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Fuel { get; set; }
        public string? Transmission { get; set; }
        public double? PriceMin { get; set; }
        public double? PriceMax { get; set; }
    }
}
