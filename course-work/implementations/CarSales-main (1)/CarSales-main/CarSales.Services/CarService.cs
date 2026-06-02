using System;
using CarSales.Contracts.DTOs.Request.Car;
using CarSales.Contracts.DTOs.Response.Car;
using CarSales.Contracts.Interfaces;
using CarSales.Data.Entities;
using CarSales.Data.Persistance;
using CarSales.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _repository;
        private readonly CarSalesDbContext _context;

        public CarService(ICarRepository repository, CarSalesDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<Car?> GetByIdAsync(Guid id)
        {
            var cars = await _repository.GetByIdAsync(id);
            return cars;
        }
        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            var cars = await _repository.GetAllAsync();
            return cars;
        }
        public async Task AddAsync(Car item)
        {
            await _repository.AddAsync(item);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Car item)
        {
            _repository.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Car item)
        {
            _repository.Delete(item);
            await _context.SaveChangesAsync();
        }
        
        public List<CarListResponse> MapToListResponse(IEnumerable<Car> cars)
        {
            return cars.Select(c => new CarListResponse
            {
                Id = c.Id,
                Brand = c.Brand.ToString(),
                Model = c.Model,
                Year = c.Year,
                Price = c.Price,
                Fuel = c.Fuel.ToString(),
                CreatedAt = c.CreatedAt,
                MainPhotoUrl = c.Photos != null && c.Photos.Any() ? c.Photos.OrderBy(p => p.ImageOrder).FirstOrDefault()?.ImagePath : null,
            }).ToList();
        }
        
        public async Task<CarPageResponse> GetAllCarsPagedAsync(CarPageRequest request, bool isAdmin)
        {
            var page = Math.Max(request.Page, 1);
            var pageSize = Math.Clamp(request.PageSize, 1, 50);

            var query = _repository.GetAllAsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Brand))
            {
                query = query.Where(c => c.Brand.ToString().Contains(request.Brand));
            }

            if (!string.IsNullOrWhiteSpace(request.Model))
            {
                query = query.Where(c => c.Model.Contains(request.Model));
            }

            if (!string.IsNullOrWhiteSpace(request.Fuel))
            {
                query = query.Where(c => c.Fuel.ToString().Contains(request.Fuel));
            }

            if (!string.IsNullOrWhiteSpace(request.Transmission))
            {
                query = query.Where(c => c.Transmission.ToString().Contains(request.Transmission));
            }

            if (!string.IsNullOrWhiteSpace(request.PriceMin.ToString()))
            {
                query = query.Where(c => c.Price >= request.PriceMin);
            }

            if (!string.IsNullOrWhiteSpace(request.PriceMax.ToString()))
            {
                query = query.Where(c => c.Price <= request.PriceMax);
            }

            query = request.IsDescending
                ? query.OrderByDescending(c => c.CreatedAt)
                : query.OrderBy(c => c.CreatedAt);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CarListResponse
                {
                    Id = c.Id,
                    Brand = c.Brand.ToString(),
                    Model = c.Model,
                    Year = c.Year,
                    Price = c.Price,
                    Fuel = c.Fuel.ToString(),
                    CreatedAt = c.CreatedAt,
                    MainPhotoUrl = c.Photos != null && c.Photos.Any() ? c.Photos.OrderBy(p => p.ImageOrder).Select(p => p.ImagePath).FirstOrDefault() : null
                }).ToListAsync();

            return new CarPageResponse
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                IsDescending = request.IsDescending,
                Brand = request.Brand,
                Model = request.Model,
                Fuel = request.Fuel,
                Transmission = request.Transmission,
                PriceMin = request.PriceMin,
                PriceMax = request.PriceMax,
            };
        }

        public CarDetailedResponse MapToDetailedResponse(Car car)
        {
            return new CarDetailedResponse
            {
                Id = car.Id,
                Brand = car.Brand.ToString(),
                Model = car.Model,
                Year = car.Year,
                Price = car.Price,
                Fuel = car.Fuel.ToString(),
                Transmission = car.Transmission.ToString(),
                Color = car.Color.ToString(),
                Power = car.Power,
                EngineVolume = car.EngineVolume,
                Description = car.Description,
                CreatedAt = car.CreatedAt,
                LastChanged = car.LastChange,
                PhotoUrls = car.Photos != null ? car.Photos.OrderBy(p => p.ImageOrder).Select(p => p.ImagePath).ToList() : new List<string>(),
            };
        }

        public Car CreateCar(CarRequest request, Guid id)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            DateTime creationTime = DateTime.UtcNow;
            return new Car
            {
                Id = Guid.NewGuid(),
                CreatedAt = creationTime,
                LastChange = creationTime,
                Brand = request.Brand,
                Model = request.Model,
                Year = request.Year,
                Price = request.Price,
                Fuel = request.Fuel,
                Transmission = request.Transmission,
                Color = request.Color,
                Power = request.Power,
                EngineVolume = request.EngineVolume,
                Description = request.Description,
                UserId = id,
            };
        }

        public async Task<Car> UpdateCar(CarRequest model, Car car)
        {
            car.Brand = model.Brand;
            car.Model = model.Model;
            car.Year = model.Year;
            car.Price = model.Price;
            car.Fuel = model.Fuel;
            car.Transmission = model.Transmission;
            car.Color = model.Color;
            car.Power = model.Power;
            car.EngineVolume = model.EngineVolume;
            car.Description = model.Description;
            car.LastChange = DateTime.UtcNow;

            return car;
        }

        public CarUpdatedResponse MapToCarUpdatedResponse(Car car)
        {
            return new CarUpdatedResponse
            {
                Id = car.Id,
                Brand = car.Brand.ToString(),
                Model = car.Model,
                Year = car.Year,
                Price = car.Price,
                Fuel = car.Fuel.ToString(),
                CreatedAt = car.CreatedAt,
                LastChanged = car.LastChange,
                Desription = car.Description,
            };
        }
    }
}
