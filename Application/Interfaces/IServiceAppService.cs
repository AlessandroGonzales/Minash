using Application.DTO;

namespace Application.Interfaces
{
    public interface IServiceAppService
    {
        Task<IEnumerable<ServiceDto>> GetAllServicesAsync();
        Task<ServiceDto?> GetServiceByIdAsync(int id);
        Task<IEnumerable<ServiceDto>> GetServiceByNameAsync(string name);
        Task<IEnumerable<ServiceDto>> GetServicesByQualityAsync(string quality);
        Task<ServiceDto> AddServiceAsync(ServiceDto serviceDto);
        Task UpdateServiceAsync(ServiceDto serviceDto);
        Task DeleteServiceAsync(int id);
    }
}
