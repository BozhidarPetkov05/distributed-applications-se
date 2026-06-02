using Microsoft.AspNetCore.Http;

namespace CarSales.Contracts.DTOs.Request
{
    public class FileUpload
    {
        public IFormFile File { get; set; }
        public Guid CarId { get; set; }
    }
}
