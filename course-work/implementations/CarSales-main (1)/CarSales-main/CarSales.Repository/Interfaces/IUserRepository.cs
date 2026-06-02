using CarSales.Data.Entities;

namespace CarSales.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> UsernameExists(string username);
        IQueryable<User> GetAllAsQueryable();
    }
}
