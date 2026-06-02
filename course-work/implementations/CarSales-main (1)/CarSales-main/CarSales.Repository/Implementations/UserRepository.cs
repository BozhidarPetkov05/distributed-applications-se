using CarSales.Data.Entities;
using CarSales.Data.Persistance;
using CarSales.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Repository.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(CarSalesDbContext context) : base(context)
        {
        }

        public IQueryable<User> GetAllAsQueryable()
        {
            return _items;
        }

        public override async Task<User?> GetByIdAsync(Guid id)
        {
            return await _items.Include(u => u.Cars).ThenInclude(c => c.Photos).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> UsernameExists(string username)
        {
            return await _items.AnyAsync(u => u.Username == username);
        }
    }
}
