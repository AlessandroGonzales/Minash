using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IServiceAppService
    {
        Task<IEnumerable<ServiceResponse>> GetAllServicesAsync();
        Task<ServiceResponse?> GetServiceByIdAsync(int id);
        Task<IEnumerable<ServiceResponse>> GetServiceByNameAsync(string name);
        Task<IEnumerable<ServiceResponse>> GetServicesByQualityAsync(string quality);
        Task<ServiceResponse> AddServiceAsync(ServiceRequest serviceDto, string webRootPath);
        Task UpdateServiceAsync(int id, ServiceRequest serviceDto);
        Task PartialUpdateServiceAsync(int id, ServicePartial serviceDto);
        Task DeleteServiceAsync(int id);
    }
}
