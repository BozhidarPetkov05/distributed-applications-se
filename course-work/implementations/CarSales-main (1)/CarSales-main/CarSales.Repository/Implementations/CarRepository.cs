using CarSales.Data.Entities;
using CarSales.Data.Persistance;
using CarSales.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Repository.Implementations
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        public CarRepository(CarSalesDbContext context) : base(context)
        {
        }

        public IQueryable<Car> GetAllAsQueryable()
        {
            return _items;
        }
        public override async Task<Car?> GetByIdAsync(Guid id)
        {
            return await _items.Include(c => c.Photos).FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
