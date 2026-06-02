using CarSales.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CarSales.Data.Persistance
{
    public class CarSalesDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<User> Users { get; set; }

        public CarSalesDbContext(DbContextOptions<CarSalesDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region User
            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Id = Guid.Parse("d0b4e918-06b0-4e53-b10d-31331a02c1e5"),
                    CreatedAt = new DateTime(2026, 5, 13),
                    LastChange = new DateTime(2026, 5, 13),
                    Username = "admin",
                    Password = "admin",
                    FirstName = "Admin",
                    LastName = "Admin",
                    Age = 1,
                    IsAdmin = true,
                });
            #endregion

            var foreignKeys = modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())
                .ToList();

            foreach (var fk in foreignKeys)
            {
                fk.DeleteBehavior = DeleteBehavior.Cascade;
            }
            
        }
    }
}
