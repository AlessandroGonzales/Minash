using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface ICustomAppService
    {
        Task<IEnumerable<CustomResponse>> GetAllCustomsAsync();
        Task<CustomResponse> GetCustomByIdAsync(int id);
        Task<CustomResponse> AddCustomAsync(CustomRequest customDto);
        Task UpdateCustomAsync(CustomRequest customDto);
        Task DeleteCustomAsync(int id);
    }
}
