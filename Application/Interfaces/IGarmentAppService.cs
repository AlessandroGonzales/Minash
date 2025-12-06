using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IGarmentAppService
    {
        Task<IEnumerable<GarmentResponse>> GetAllGarmentsAsync();
        Task<GarmentResponse?> GetGarmentByIdAsync(int id);
        Task<IEnumerable<GarmentResponse>> GetGarmentsByNameAsync(string name);
        Task<GarmentResponse> AddGarmentAsync(GarmentRequest garmentDto, string webRootPath);
        Task UpdateGarmentAsync(int id, GarmentRequest garmentDto);
        Task PartialUpdateGarmentAsync(int id, GarmentPartial garmentDto);
        Task DeleteGarmentAsync(int id);
    }
}
