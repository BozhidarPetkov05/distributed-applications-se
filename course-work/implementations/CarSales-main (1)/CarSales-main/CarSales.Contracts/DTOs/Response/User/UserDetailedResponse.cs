using CarSales.Contracts.DTOs.Response.Car;

namespace CarSales.Contracts.DTOs.Response.User
{
    public class UserDetailedResponse : BaseUserResponse
    {
        public int? Age { get; set; }
        public ICollection<CarListResponse> Cars { get; set; } = new List<CarListResponse>();
    }
}
