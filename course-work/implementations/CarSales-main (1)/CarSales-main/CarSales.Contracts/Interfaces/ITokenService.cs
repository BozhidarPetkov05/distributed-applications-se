using CarSales.Data.Entities;

namespace CarSales.Contracts.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(User user);
    }
}
