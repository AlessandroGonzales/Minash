using Domain.Entities;

namespace Domain.Repositories;
public interface IServiceRepository
{
    Task<IEnumerable<Service>> GetAllServicesAsync();
    Task<Service?> GetServiceByIdAsync(int id);
    Task<IEnumerable<Service>> GetServicesByNameAsync(string name);
    Task<Service> AddServiceAsync(Service service);
    Task UpdateServiceAsync(Service service);
    Task DeleteServiceAsync(int id);
}