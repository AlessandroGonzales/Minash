using Domain.Entities;

namespace Domain.Repositories
{
    public interface ICustomRepository
    {
        Task<IEnumerable<Custom>> GetAllCustomsAsync();
        Task<Custom> GetCustomByIdAsync(int id);
        Task<Custom> AddCustomAsync(Custom custom);
        Task UpdateCustomAsync(Custom custom);
        Task DeleteCustomAsync(int id);
    }
}
