using Application.DTO;

namespace Application.Interfaces
{
    public interface IGarmentServiceAppService
    {
        Task<IEnumerable<GarmentServiceDto>> GetAllGarmentServicesAsync();
        Task<GarmentServiceDto?> GetGarmentServiceByIdAsync(int id);
        Task<IEnumerable<GarmentServiceDto>> GetGarmentServicesByGarmentIdAsync(int garmentId);
        Task<IEnumerable<GarmentServiceDto>> GetGarmentServicesByServiceIdAsync(int serviceId);
        Task<GarmentServiceDto> AddGarmentServiceAsync(GarmentServiceDto garmentServiceDto);
        Task UpdateGarmentServiceAsync(GarmentServiceDto garmentServiceDto);
        Task DeleteGarmentServiceAsync(int id);
    }
}
