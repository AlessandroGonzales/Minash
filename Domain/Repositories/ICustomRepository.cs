using Domain.Entities;

namespace Domain.Repositories
{
    public interface ICustomRepository
    {
        Task<IEnumerable<Custom>> GetAllCustomsAsync();
        Task<IEnumerable<Custom>> GetCustomsByUserNameAsync(string userName);
        Task<IEnumerable<Custom>> GetCustomsByOrderIdAsync(int orderId);
        Task<Custom?> GetCustomByIdAsync(int id);
        Task<Custom> AddCustomAsync(Custom custom);
        Task UpdateCustomAsync(int id, Custom custom);
        Task PartialUpdateCustomAsync(int id, Custom custom);
        Task DeleteCustomAsync(int id);
    }
}
