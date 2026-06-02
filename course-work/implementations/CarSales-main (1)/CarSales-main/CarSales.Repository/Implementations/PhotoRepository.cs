using CarSales.Data.Entities;
using CarSales.Data.Persistance;
using CarSales.Repository.Interfaces;

namespace CarSales.Repository.Implementations
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        public PhotoRepository(CarSalesDbContext context) : base(context)
        {
        }
    }
}
