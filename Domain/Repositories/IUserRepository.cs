using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersByRolIdAsync(int roleId);
        Task<IEnumerable<User>> GetUsersByCityAsync(string city);
        Task<User?> GetUserByNameAsync(string name);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> AddUserAsync(User user);
        Task UpdateUserAsync(int id, User user);
        Task UpdateUserNameAndPhoneAsync(int id, User user);
        Task DeleteUserAsync(int id);
    }
}
