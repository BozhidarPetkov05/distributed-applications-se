namespace CarSales.Contracts.DTOs.Response.Car
{
    public class CarDetailedResponse
    {
        public Guid Id { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public required double Price { get; set; }
        public required string Fuel { get; set; }
        public required string Transmission { get; set; }
        public required string Color { get; set; }
        public int Power { get; set; }
        public int EngineVolume { get; set; }
        public string? Description { get; set; }
        public ICollection<string> PhotoUrls { get; set; } = new List<string>();

        public DateTime CreatedAt { get; set; }
        public DateTime LastChanged { get; set; }
    }
}
