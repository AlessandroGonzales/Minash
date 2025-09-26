using Application.DTO;

namespace Application.Interfaces
{
    public interface IGarmentAppService
    {
        Task<IEnumerable<GarmentDto>> GetAllGarmentsAsync();
        Task<GarmentDto?> GetGarmentByIdAsync(int id);
        Task<IEnumerable<GarmentDto>> GetGarmentsByNameAsync(string name);
        Task<GarmentDto> AddGarmentAsync(GarmentDto garmentDto);
        Task UpdateGarmentAsync(GarmentDto garmentDto);
        Task DeleteGarmentAsync(int id);
    }
}
