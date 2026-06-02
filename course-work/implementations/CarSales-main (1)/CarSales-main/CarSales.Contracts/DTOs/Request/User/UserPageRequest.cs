namespace CarSales.Contracts.DTOs.Request.User
{
    public class UserPageRequest : PageRequest
    {
        public string? Username { get; set; }
        public bool? IsAdmin { get; set; }
    }
}
