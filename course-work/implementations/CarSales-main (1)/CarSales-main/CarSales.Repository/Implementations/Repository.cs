using CarSales.Data.Entities;
using CarSales.Data.Persistance;
using CarSales.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly CarSalesDbContext _context;
        protected readonly DbSet<T> _items;

        public Repository(CarSalesDbContext context)
        {
            _context = context;
            _items = _context.Set<T>();
        }
        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _items.FindAsync(id);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _items.ToListAsync();
        }
        public async Task AddAsync(T entity)
        {
            await _items.AddAsync(entity);
        }
        public void Update(T entity)
        {
            _items.Update(entity);
        }
        public void Delete(T entity)
        {
            _items.Remove(entity);
        }

    }
}
