using CarSales.Data.Entities;

namespace CarSales.Contracts.Interfaces
{
    public interface IPhotoService
    {
        Task AddAsync(Photo item);
    }
}
