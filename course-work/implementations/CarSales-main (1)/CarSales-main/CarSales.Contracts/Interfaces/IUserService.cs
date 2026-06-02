using CarSales.Contracts.DTOs.Request.User;
using CarSales.Contracts.DTOs.Response.User;
using CarSales.Data.Entities;

namespace CarSales.Contracts.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task AddAsync(User item);
        Task UpdateAsync(User item);
        Task DeleteAsync(User item);
        List<UserListResponse> MapToListResponse(IEnumerable<User> users);
        UserDetailedResponse MapToDetailedResponse(User user);
        Task<bool> UsernameExists(string username);
        User CreateUser(UserRequest model);
        User UpdateUser(UserRequest model, User user);
        UpdatedUserResponse MapToUpdatedUserResponse(User user);

        Task<UserPageResponse> GetAllUsersPagedAsync(UserPageRequest request);
    }
}
