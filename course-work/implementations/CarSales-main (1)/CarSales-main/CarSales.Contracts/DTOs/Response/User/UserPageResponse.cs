namespace CarSales.Contracts.DTOs.Response.User
{
    public class UserPageResponse : PageResponse<UserListResponse>
    {
        public string? Username { get; set; }
    }
}
