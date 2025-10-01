using Application.DTO;

namespace Application.Interfaces
{
    public interface ICustomAppService
    {
        Task<IEnumerable<CustomDto>> GetCustomAppsAsync();
        Task<CustomDto> GetCustomAppByIdAsync(int id);
        Task<CustomDto> AddCustomAsync(CustomDto customDto);
        Task UpdateCustomAsync(CustomDto customDto);
        Task DeleteCustomAsync(int id);
    }
}
