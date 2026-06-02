using CarSales.Data.Entities;

namespace CarSales.Repository.Interfaces
{
    public interface ICarRepository : IRepository<Car>
    {
        IQueryable<Car> GetAllAsQueryable();
    }
}
