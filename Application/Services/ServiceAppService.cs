using Application.Interfaces;
using Domain.Repositories;
using Domain.Entities;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.DTO.Partial;

namespace Application.Services
{
    public class ServiceAppService : IServiceAppService
    {
        private readonly IServiceRepository _repo;
        private readonly IFileStorageService _fileStorage;
        public ServiceAppService(IServiceRepository repo, IFileStorageService fileStorage)
        {
            _repo = repo;
            _fileStorage = fileStorage;
        }

        private static ServiceResponse MapToResponse(Service d) => new ServiceResponse
        {
            IdService = d.IdService,
            ServiceName = d.ServiceName,
            ServiceDetails = d.ServiceDetails,
            ServicePrice = d.Price,
            ImageUrl = d.ImageUrl,
        };  

        private static Service MapToDomain(ServiceRequest dto) => new Service
        {
            IdService = dto.IdService,
            ServiceName = dto.ServiceName,
            ServiceDetails = dto.ServiceDetails,
            Price = dto.Price,
        };

        private static Service MapToDomain(ServicePartial dto) => new Service
        {
            Price = dto.ServicePrice,
        };

        public async Task<IEnumerable<ServiceResponse>> GetAllServicesAsync()
        {
            var list = await _repo.GetAllServicesAsync();
            return list.Select(MapToResponse);
        }

        public async Task<ServiceResponse?> GetServiceByIdAsync(int id)
        {
            var service = await _repo.GetServiceByIdAsync(id);
            return service == null ? null : MapToResponse(service);
        }

        public async Task<IEnumerable<ServiceResponse>> GetServiceByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Enumerable.Empty<ServiceResponse>();

            var normalizedName = name.Replace(" ", "").ToLower();
            var list = await _repo.GetServicesByNameAsync(normalizedName);
            return list.Select(MapToResponse);
        }

        public async Task<IEnumerable<ServiceResponse>> GetServicesByQualityAsync(string quality)
        {
            IEnumerable<Service> services = quality switch
            {
                "premium" => await _repo.GetServicesByPriceAsync(500, 1000),
                "standard" => await _repo.GetServicesByPriceAsync(100, 499),
                "basic" => await _repo.GetServicesByPriceAsync(10, 99),
                _ => Enumerable.Empty<Service>()
            };

            return services.Select(MapToResponse);

        }

        public async Task<ServiceResponse> AddServiceAsync(ServiceRequest service, string webRootPath)
        {
            string imageUrl = null;
            if(service.ImageUrl != null)
            {
                imageUrl = await _fileStorage.UploadFileAsync(service.ImageUrl, "services", webRootPath);
            }
            
            var domainService = MapToDomain(service);
            domainService.ImageUrl = imageUrl;
            var createdService = await _repo.AddServiceAsync(domainService);
            return MapToResponse(createdService);
        }

        public async Task UpdateServiceAsync(int id , ServiceRequest service)
        {
            var domainService = MapToDomain(service);
            await _repo.UpdateServiceAsync( id, domainService);
        }

        public async Task PartialUpdateServiceAsync(int id, ServicePartial service)
        {
            var domainService = MapToDomain(service);
            await _repo.PartialUpdateServiceAsync(id, domainService);

        }
        public async Task DeleteServiceAsync(int id)
        {
            await _repo.DeleteServiceAsync(id);
        }
    }
}
