namespace CarSales.Contracts.DTOs.Request
{
    public class PageRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 8;
        public bool IsDescending { get; set; } = true;
    }
}
