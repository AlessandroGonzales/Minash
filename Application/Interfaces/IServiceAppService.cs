using Application.DTO;

namespace Application.Interfaces
{
    public interface IServiceAppService
    {
        Task<IEnumerable<ServiceDto>> GetAllServicesAsync();
        Task<ServiceDto?> GetServiceByIdAsync(int id);
        Task<ServiceDto> AddServiceAsync(ServiceDto serviceDto);
        Task UpdateServiceAsync(ServiceDto serviceDto);
        Task DeleteServiceAsync(int id);
    }
}
