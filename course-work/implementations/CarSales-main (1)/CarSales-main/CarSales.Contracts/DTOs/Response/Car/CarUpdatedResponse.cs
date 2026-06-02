namespace CarSales.Contracts.DTOs.Response.Car
{
    public class CarUpdatedResponse
    {
        public Guid Id { get; set; }
        public required string Brand { get; set; }
        public required string Model { get; set; }
        public int Year { get; set; }
        public double Price { get; set; }
        public required string Fuel { get; set; }
        public string Desription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastChanged { get; set; }
    }
}
