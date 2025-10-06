using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IUserAppService
    {
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
        Task<UserResponse?> GetUserByIdAsync(int id);
        Task<UserResponse?> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserResponse>> GetUsersByRolIdAsync(int roleId);
        Task<UserResponse> AddUserAsync(UserRequest userDto);
        Task UpdateUserAsync(UserRequest userDto);
        Task DeleteUserAsync(int id);
    }
}
