namespace CarSales.Contracts.DTOs.Response.User
{
    public class BaseUserResponse
    {
        public Guid Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastChanged { get; set; }
        public bool IsAdmin { get; set; }
    }
}
