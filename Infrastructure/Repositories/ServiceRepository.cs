using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence; // MinashDbContext namespace
using Microsoft.EntityFrameworkCore;
using EfService = Infrastructure.Persistence.Entities.Service;

namespace Infrastructure.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly MinashDbContext _db;
        public ServiceRepository(MinashDbContext db)
        {
            _db = db;
        }

        private static Service MapToDomain(EfService efService) => new Service
        {
            IdService = efService.IdService,
            ServiceName = efService.ServiceName,
            ServiceDetails = efService.ServiceDetails,
            Price = efService.Price,
            ImageUrl = efService.ImageUrl ?? string.Empty,
            CreatedAt = efService.CreatedAt.HasValue ? DateTime.SpecifyKind(efService.CreatedAt.Value, DateTimeKind.Utc) : DateTime.MinValue,
            UpdatedAt = efService.UpdatedAt.HasValue ? DateTime.SpecifyKind(efService.UpdatedAt.Value, DateTimeKind.Utc) : DateTime.MinValue,
        };

        private static EfService MaptoEf(Service d) => new EfService
        {
            ServiceName = d.ServiceName,
            ServiceDetails = d.ServiceDetails,
            Price = d.Price,
            ImageUrl = d.ImageUrl,
        };

        public async Task<IEnumerable<Service>> GetAllServicesAsync()
        { 
            var list = await _db.Services.ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task<Service?> GetServiceByIdAsync(int id)
        {
            var efService = await _db.Services.FindAsync(id);
            return efService == null ? null : MapToDomain(efService);
        }

        public async Task<IEnumerable<Service>> GetServicesByNameAsync(string name)
        {
            var list = await _db.Services
                .Where(s => s.ServiceName.Replace(" ", "").ToLower().Contains(name))
                .ToListAsync();
            return list.Select(MapToDomain);
        }
        public async Task<Service> AddServiceAsync(Service service)
        { 
            var efService = MaptoEf(service);
            _db.Services.Add(efService);
            await _db.SaveChangesAsync();
            service.IdService = efService.IdService;
            return service;
        }

        public async Task UpdateServiceAsync(Service service)
        {
            var ef = await _db.Services.FindAsync(service.IdService);
            if (ef == null) throw new KeyNotFoundException($"Service {service.IdService} not found.");

            // Actualizar campos permitidos
            ef.ServiceName = service.ServiceName;
            ef.ServiceDetails = service.ServiceDetails;
            ef.Price = service.Price;

            _db.Services.Update(ef);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteServiceAsync(int id)
        {
            var efService = await _db.Services.FindAsync(id);
            if (efService == null) throw new KeyNotFoundException($"Service {id} not found.");
            _db.Services.Remove(efService);
            await _db.SaveChangesAsync();
        }
    }
}
