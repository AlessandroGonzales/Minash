using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface ICustomAppService
    {
        Task<IEnumerable<CustomResponse>> GetAllCustomsAsync();
        Task<IEnumerable<CustomResponse>> GetCustomsByUserNameAsync(string userName);
        Task<CustomResponse?> GetCustomByIdAsync(int id);
        Task<CustomResponse> AddCustomAsync(int userId, CustomRequest customDto, string webRootPath);
        Task<IEnumerable<CustomResponse>> GetCustomsByOrderIdAsync(int orderId);
        Task UpdateCustomAsync(int id, CustomRequest customDto);
        Task PartialUpdateCustomAsync(int id, CustomPartial customDto);
        Task DeleteCustomAsync(int id);
    }
}
