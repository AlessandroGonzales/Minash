using Domain.Entities;

namespace Domain.Repositories
{
    public interface IGarmentServiceRepository
    {
        Task<IEnumerable<GarmentService>> GetGarmentsServiceByGarmentIdAsync(int garmentId);
        Task<IEnumerable<GarmentService>> GetGarmentsServiceByServiceIdAsync(int serviceId);
        Task<GarmentService> AddGarmentServiceAsync(GarmentService garmentService);
        Task<IEnumerable<GarmentService>> GetAllGarmentServicesAsync();
        Task<GarmentService?> GetGarmentServiceByIdAsync(int id);
        Task DeleteGarmentServiceAsync(int id);
        Task UpdateGarmentServiceAsync(GarmentService garmentService);

    }
}
