using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class GarmentServiceAppService : IGarmentServiceAppService
    {
        private readonly IGarmentServiceRepository _repo;
        public GarmentServiceAppService(IGarmentServiceRepository repo)
        {
            _repo = repo;
        }

        private static GarmentServiceDto MapToDto(GarmentService d) => new GarmentServiceDto
        {
            IdGarmentService = d.IdGarmentService,
            IdGarment = d.IdGarment,
            IdService = d.IdService,
            AdditionalPrice = d.AdditionalPrice,
            ImageUrl = d.ImageUrl,
            CreatedAt = d.CreatedAt,
            UpdatedAt = d.UpdatedAt,
        };

        private static GarmentService MapToDomain(GarmentServiceDto dto) 
        {

            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new GarmentService
            {
                IdGarmentService = dto.IdGarmentService,
                IdGarment = dto.IdGarment,
                IdService = dto.IdService,
                AdditionalPrice = dto.AdditionalPrice,
                ImageUrl = dto.ImageUrl,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
            };
        }

        public async Task<IEnumerable<GarmentServiceDto>> GetAllGarmentServicesAsync()
        {
            var list = await _repo.GetAllGarmentServicesAsync();
            return list.Select(MapToDto);
        }

        public async Task<GarmentServiceDto?> GetGarmentServiceByIdAsync(int id)
        {
            var domain = await _repo.GetGarmentServiceByIdAsync(id);
            return domain != null ? MapToDto(domain) : null;
        }

        public async Task<IEnumerable<GarmentServiceDto>> GetGarmentServicesByGarmentIdAsync(int garmentId)
        {
            var domainList = await _repo.GetGarmentsServiceByGarmentIdAsync(garmentId); 
            return domainList.Select(MapToDto);
        }

        public async Task<IEnumerable<GarmentServiceDto>> GetGarmentServicesByServiceIdAsync(int serviceId)
        {
            var domainList = await _repo.GetGarmentsServiceByServiceIdAsync(serviceId);
            return domainList.Select(MapToDto);
        }

        public async Task<GarmentServiceDto> AddGarmentServiceAsync(GarmentServiceDto dto)
        {
            if (dto.IdGarment <= 0 || dto.IdService <= 0)
                throw new ArgumentException("IDs de Garment y Service deben ser mayores a 0.");

            var domain = MapToDomain(dto);
            domain.CreatedAt = DateTime.UtcNow; 
            domain.UpdatedAt = DateTime.UtcNow;

            var addedDomain = await _repo.AddGarmentServiceAsync(domain);
            return MapToDto(addedDomain);
        }

        public async Task UpdateGarmentServiceAsync(GarmentServiceDto dto)
        {
            if (dto.IdGarmentService <= 0)
                throw new ArgumentException("ID de GarmentService debe ser mayor a 0.");

            var domain = MapToDomain(dto);
            domain.UpdatedAt = DateTime.UtcNow; 

            await _repo.UpdateGarmentServiceAsync(domain);
        }

        public async Task DeleteGarmentServiceAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID debe ser mayor a 0.");

            await _repo.DeleteGarmentServiceAsync(id);
        }
    }
}
