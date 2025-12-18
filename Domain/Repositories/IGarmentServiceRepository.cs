using Domain.Entities;

namespace Domain.Repositories
{
    public interface IGarmentServiceRepository
    {
        Task<IEnumerable<GarmentService>> GetAllGarmentServicesAsync();
        Task<IEnumerable<GarmentService>> GetGarmentServiceOneImageAsync(int count);
        Task<IEnumerable<GarmentService>> GetGarmentsServiceByGarmentIdAsync(int garmentId);
        Task<IEnumerable<GarmentService>> GetGarmentsServiceByServiceIdAsync(int serviceId);
        Task<IEnumerable<GarmentService>> GetGarmentServicesByPriceAsync(decimal priceMin, decimal priceMax);
        Task<GarmentService> AddGarmentServiceAsync(GarmentService garmentService);
        Task<GarmentService?> GetGarmentServiceByIdAsync(int id);
        Task UpdateGarmentServiceAsync(int id, GarmentService garmentService);
        Task PartialUpdateGarmentServiceAsync(int id, GarmentService garmentService);
        Task DeleteGarmentServiceAsync(int id);

    }
}
