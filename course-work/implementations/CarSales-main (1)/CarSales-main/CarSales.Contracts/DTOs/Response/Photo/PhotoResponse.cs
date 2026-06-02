namespace CarSales.Contracts.DTOs.Response.Photo
{
    public class PhotoResponse
    {
        public Guid Id { get; set; }
        public required string ImagePath { get; set; }
        public bool IsMain { get; set; }
        public int ImageOrder { get; set; }
    }
}
