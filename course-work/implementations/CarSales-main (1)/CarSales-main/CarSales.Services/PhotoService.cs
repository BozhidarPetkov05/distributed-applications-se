using CarSales.Contracts.Interfaces;
using CarSales.Data.Entities;
using CarSales.Data.Persistance;
using CarSales.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CarSales.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRepository _repository;
        private readonly CarSalesDbContext _context;

        public PhotoService(IPhotoRepository repository, CarSalesDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task AddAsync(Photo item)
        {
            if (item.CarId != Guid.Empty)
            {
                var existingPhotos = await _context.Set<Photo>()
                    .Where(p => p.CarId == item.CarId)
                    .ToListAsync();

                item.ImageOrder = existingPhotos.Any() ? existingPhotos.Max(p => p.ImageOrder) + 1 : 1;

                var hasMain = existingPhotos.Any(p => p.IsMain);
                item.IsMain = !hasMain;
            }

            await _repository.AddAsync(item);
            await _context.SaveChangesAsync();

            // Ensure uniqueness of IsMain for the car
            if (item.IsMain)
            {
                var otherMains = await _context.Set<Photo>()
                    .Where(p => p.CarId == item.CarId && p.Id != item.Id && p.IsMain)
                    .ToListAsync();

                if (otherMains.Any())
                {
                    foreach (var p in otherMains)
                        p.IsMain = false;

                    _context.UpdateRange(otherMains);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
