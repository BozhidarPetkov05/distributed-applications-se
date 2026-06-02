using CarSales.Contracts.DTOs.Request.User;
using CarSales.Contracts.DTOs.Response.Car;
using CarSales.Contracts.DTOs.Response.User;
using CarSales.Contracts.Interfaces;
using CarSales.Data.Entities;
using CarSales.Data.Persistance;
using CarSales.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repository;
        private readonly CarSalesDbContext _context;
        public UserService(IUserRepository repository, CarSalesDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        public async Task<User?> GetByIdAsync(Guid id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user is null)
            {
                return null;
            }

            return user;
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _repository.GetAllAsync();
            return users;
        }
        public async Task AddAsync(User item)
        {
            await _repository.AddAsync(item);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(User item)
        {
            _repository.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User item)
        {
            var user = await _context.Users
                .Include(u => u.Cars)
                .FirstOrDefaultAsync(u => u.Id == item.Id);

            if (user is null)
            {
                return;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public List<UserListResponse> MapToListResponse(IEnumerable<User> users)
        {
            return users.Select(u => new UserListResponse
            {
                Id = u.Id,
                Username = u.Username,
                Password = u.Password,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsAdmin = u.IsAdmin,
                CreatedAt = u.CreatedAt,
                LastChanged = u.LastChange
            }).ToList();
        }

        public UserDetailedResponse MapToDetailedResponse(User user)
        {
            return new UserDetailedResponse 
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                IsAdmin = user.IsAdmin,
                CreatedAt = user.CreatedAt,
                LastChanged = user.LastChange,
                Cars = user.Cars.Select(c => new CarListResponse
                {
                    Id = c.Id,
                    Brand = c.Brand.ToString(),
                    Model = c.Model,
                    Year = c.Year,
                    Price = c.Price,
                    Fuel = c.Fuel.ToString(),
                    CreatedAt = c.CreatedAt,
                    MainPhotoUrl = c.Photos
                        .FirstOrDefault(p => p.IsMain)?.ImagePath
                }).ToList()
            };
        }

        public async Task<bool> UsernameExists(string username)
        {
            return await _repository.UsernameExists(username);
        }

        public User CreateUser(UserRequest model)
        {
            DateTime creationTime = DateTime.UtcNow;
            return new User 
            { 
                Id = Guid.NewGuid(),
                CreatedAt = creationTime,
                LastChange = creationTime,
                Username = model.Username,
                Password = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Age = model.Age,
                IsAdmin = false
            };
        }
        public User UpdateUser(UserRequest model, User user)
        {
            user.Username = model.Username;
            user.Password = model.Password;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            if (model.Age is not null)
            {
                user.Age = model.Age;
            }
            if (model.IsAdmin.HasValue)
            {
                user.IsAdmin = model.IsAdmin.Value;
            }

            user.LastChange = DateTime.UtcNow;

            return user;
        }

        public UpdatedUserResponse MapToUpdatedUserResponse(User user)
        {
            return new UpdatedUserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                IsAdmin = user.IsAdmin,
                CreatedAt = user.CreatedAt,
                LastChanged = user.LastChange,
            };
        }
        public async Task<UserPageResponse> GetAllUsersPagedAsync(UserPageRequest request)
        {
            var page = Math.Max(request.Page, 1);
            var pageSize = Math.Clamp(request.PageSize, 1, 50);

            var query = _repository.GetAllAsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                query = query.Where(u => u.Username.Contains(request.Username));
            }

            if (request.IsAdmin.HasValue)
            {
                query = query.Where(u => u.IsAdmin == request.IsAdmin.Value);
            }

            query = request.IsDescending
                ? query.OrderByDescending(u => u.CreatedAt)
                : query.OrderBy(u => u.CreatedAt);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserListResponse
                {
                    Id = u.Id,
                    Username = u.Username,
                    Password = u.Password,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    IsAdmin = u.IsAdmin,
                    CreatedAt = u.CreatedAt,
                    LastChanged = u.LastChange,
                })
                .ToListAsync();

            return new UserPageResponse
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                IsDescending = request.IsDescending,
                Username = request.Username,
            };
        }
    }
}
