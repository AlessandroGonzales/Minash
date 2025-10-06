using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IGarmentAppService
    {
        Task<IEnumerable<GarmentResponse>> GetAllGarmentsAsync();
        Task<GarmentResponse?> GetGarmentByIdAsync(int id);
        Task<IEnumerable<GarmentResponse>> GetGarmentsByNameAsync(string name);
        Task<GarmentResponse> AddGarmentAsync(GarmentRequest garmentDto);
        Task UpdateGarmentAsync(GarmentRequest garmentDto);
        Task DeleteGarmentAsync(int id);
    }
}
