using CarSales.Contracts.DTOs.Request.Car;
using CarSales.Contracts.DTOs.Response.Car;
using CarSales.Data.Entities;

namespace CarSales.Contracts.Interfaces
{
    public interface ICarService
    {
        Task<Car?> GetByIdAsync(Guid id);
        Task<IEnumerable<Car>> GetAllAsync();
        Task AddAsync(Car item);
        Task UpdateAsync(Car item);
        Task DeleteAsync(Car item);
        Car CreateCar(CarRequest request, Guid id);
        Task<Car> UpdateCar(CarRequest model, Car car);
        List<CarListResponse> MapToListResponse(IEnumerable<Car> cars);
        CarDetailedResponse MapToDetailedResponse(Car car);
        Task<CarPageResponse> GetAllCarsPagedAsync(CarPageRequest request, bool isAdmin);
        CarUpdatedResponse MapToCarUpdatedResponse(Car car);
    }
}
