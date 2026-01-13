using Application.DTO.Partial;
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
        Task<IEnumerable<GarmentServiceResponse>> GetGarmentServicesOneImageAsync(int count);
        Task<IEnumerable<GarmentServiceResponse>> GetGarmentServicesByQualityAsync(string quality);
        Task<GarmentServiceResponse> AddGarmentServiceAsync(GarmentServiceRequest garmentServiceDto, string webRootPath);
        Task UpdateGarmentServiceAsync(int id, GarmentServiceRequest garmentServiceDto);
        Task PartialUpdateGarmentServiceAsync(int id, GarmentServicePartial garmentServiceDto, string webRootPath);
        Task DeleteGarmentServiceAsync(int id);
    }
}
