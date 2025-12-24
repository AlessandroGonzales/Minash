using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IUserAppService
    {
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
        Task<IEnumerable<UserResponse>> GetUsersByCityAsync(string city);
        Task<IEnumerable<UserResponse>> GetUsersByRolIdAsync(int roleId);
        Task<UserResponse?> GetUserByNameAsync(string name);
        Task<UserResponse?> GetUserByIdAsync(int id);
        Task<UserResponse?> GetUserByEmailAsync(string email);
        Task<UserResponse> AddUserAsync(UserRequest userDto, string webRootPath);
        Task UpdateUserAsync(int id, UserRequest userDto);
        Task PartialUpdateUserAsync(int id, UserPartial userDto, string webRootPath);
        Task DeleteUserAsync(int id);
        Task<UserResponse?> ValidateUserAsync(string email, string password);
    }
}
