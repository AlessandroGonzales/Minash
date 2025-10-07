using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IGarmentServiceAppService
    {
        Task<IEnumerable<GarmentServiceResponse>> GetAllGarmentServicesAsync();
        Task<GarmentServiceResponse?> GetGarmentServiceByIdAsync(int id);
        Task<IEnumerable<GarmentServiceResponse>> GetGarmentServicesByGarmentIdAsync(int garmentId);
        Task<IEnumerable<GarmentServiceResponse>> GetGarmentServicesByServiceIdAsync(int serviceId);
        Task<IEnumerable<GarmentServiceResponse>> GetGarmentServicesByQualityAsync(string  quality);
        Task<GarmentServiceResponse> AddGarmentServiceAsync(GarmentServiceRequest garmentServiceDto);
        Task UpdateGarmentServiceAsync(GarmentServiceRequest garmentServiceDto);
        Task DeleteGarmentServiceAsync(int id);
    }
}
