using Application.Interfaces;
using Domain.Repositories;
using Application.DTO;
using Domain.Entities;

namespace Application.Services
{
    public class ServiceAppService : IServiceAppService
    {
        private readonly IServiceRepository _repo;
        public ServiceAppService(IServiceRepository repo)
        {
            _repo = repo;
        }

        private static ServiceDto MaptoDto(Service d) => new ServiceDto
        {
            IdService = d.IdService,
            ServiceName = d.ServiceName,
            ServiceDetails = d.ServiceDetails,
            Price = d.Price,
            ImageUrl = d.ImageUrl,
        };  

        private static Service MaptoDomain(ServiceDto dto) => new Service
        {
            IdService = dto.IdService,
            ServiceName = dto.ServiceName,
            ServiceDetails = dto.ServiceDetails,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl,
        };

        public async Task<IEnumerable<ServiceDto>> GetAllServicesAsync()
        {
            var list = await _repo.GetAllServicesAsync();
            return list.Select(MaptoDto);
        }

        public async Task<ServiceDto?> GetServiceByIdAsync(int id)
        {
            var service = await _repo.GetServiceByIdAsync(id);
            return service == null ? null : MaptoDto(service);
        }

        public async Task<IEnumerable<ServiceDto>> GetServiceByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Enumerable.Empty<ServiceDto>();

            var normalizedName = name.Replace(" ", "").ToLower();
            var list = await _repo.GetServicesByNameAsync(normalizedName);
            return list.Select(MaptoDto);
        }
        public async Task<ServiceDto> AddServiceAsync(ServiceDto service)
        {
            var domainService = MaptoDomain(service);
            var createdService = await _repo.AddServiceAsync(domainService);
            return MaptoDto(createdService);
        }

        public async Task UpdateServiceAsync(ServiceDto service)
        {
            var domainService = MaptoDomain(service);
            await _repo.UpdateServiceAsync(domainService);
        }

        public async Task DeleteServiceAsync(int id)
        {
            await _repo.DeleteServiceAsync(id);
        }
    }
}
