using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class GarmentServiceAppService : IGarmentServiceAppService
    {
        private readonly IGarmentServiceRepository _repo;
        private readonly IFileStorageService _fileStorage;
        public GarmentServiceAppService(IGarmentServiceRepository repo, IFileStorageService fileStorage)
        {
            _repo = repo;
            _fileStorage = fileStorage;
        }

        private static GarmentServiceResponse MapToResponse(GarmentService d) => new GarmentServiceResponse
        {
            IdGarmentService = d.IdGarmentService,
            IdGarments = d.IdGarment,
            IdService = d.IdService,
            AddtionalPrice = d.AdditionalPrice,
            Colors = d.Colors,
            Sizes =d.Sizes,
            ImageUrl = d.ImageUrl,
            GarmentServiceDetails = d.GarmentServiceDetails,
            GarmentServiceName = d.GarmentServiceName,
        };

        private static GarmentService MapToDomain(GarmentServiceRequest dto) => new GarmentService
        {
            IdGarmentService = dto.IdGarmentService,
            IdGarment = dto.IdGarment,
            IdService = dto.IdService,
            Colors = dto.Colors,
            Sizes=dto.Sizes,
            AdditionalPrice = dto.AdditionalPrice,
            GarmentServiceDetails = dto.GarmentServiceDetails,
            GarmentServiceName = dto.GarmentServiceName,
        };

        private static GarmentService MapToDomain(GarmentServicePartial dto) => new GarmentService
        {
            AdditionalPrice = dto.AdditionalPrice,
        };

        public async Task<IEnumerable<GarmentServiceResponse>> GetAllGarmentServicesAsync()
        {
            var list = await _repo.GetAllGarmentServicesAsync();
            return list.Select(MapToResponse);
        }

        public async Task<IEnumerable<GarmentServiceResponse>> GetGarmentServicesOneImageAsync(int count)
        {
            var list = await _repo.GetGarmentServiceOneImageAsync(count);
            return list.Select(MapToResponse);
        }

        public async Task<GarmentServiceResponse?> GetGarmentServiceByIdAsync(int id)
        {
            var domain = await _repo.GetGarmentServiceByIdAsync(id);
            return domain != null ? MapToResponse(domain) : null;
        }

        public async Task<IEnumerable<GarmentServiceResponse>> GetGarmentServicesByGarmentIdAsync(int garmentId)
        {
            var domainList = await _repo.GetGarmentsServiceByGarmentIdAsync(garmentId); 
            return domainList.Select(MapToResponse);
        }

        public async Task<IEnumerable<GarmentServiceResponse>> GetGarmentServicesByServiceIdAsync(int serviceId)
        {
            var domainList = await _repo.GetGarmentsServiceByServiceIdAsync(serviceId);
            return domainList.Select(MapToResponse);
        }

        public async Task<IEnumerable<GarmentServiceResponse>> GetGarmentServicesByQualityAsync(string quality)
        {
            IEnumerable<GarmentService> garmentServices = quality switch
            {
                "Premium" => await _repo.GetGarmentServicesByPriceAsync(500, 1000),
                "Standard" => await _repo.GetGarmentServicesByPriceAsync(100, 499),
                "Basic" => await _repo.GetGarmentServicesByPriceAsync(1, 99),
                _ => Enumerable.Empty<GarmentService>(),
            };

            return garmentServices.Select(MapToResponse);
        }

        public async Task<GarmentServiceResponse> AddGarmentServiceAsync(GarmentServiceRequest dto, string webRootPath)
        {
            if (dto.IdGarment <= 0 || dto.IdService <= 0)
                throw new ArgumentException("IDs de Garment y Service deben ser mayores a 0.");

            List<string> imageUrl = new List<string>();

            if(dto.ImageFiles != null && dto.ImageFiles.Count > 0)
            {
                imageUrl = await _fileStorage.UploadFilesAsync(dto.ImageFiles,"garmentServices", webRootPath);
            }
          
            var domain = MapToDomain(dto);
            domain.CreatedAt = DateTime.UtcNow; 
            domain.UpdatedAt = DateTime.UtcNow;
            domain.ImageUrl = imageUrl;

            var addedDomain = await _repo.AddGarmentServiceAsync(domain);
            return MapToResponse(addedDomain);
        }

        public async Task UpdateGarmentServiceAsync(int id, GarmentServiceRequest dto)
        {
            if (dto.IdGarmentService <= 0)
                throw new ArgumentException("ID de GarmentService debe ser mayor a 0.");

            var domain = MapToDomain(dto);
            domain.UpdatedAt = DateTime.UtcNow; 

            await _repo.UpdateGarmentServiceAsync(id, domain);
        }

        public async Task PartialUpdateGarmentServiceAsync(int id, GarmentServicePartial dto, string webRootPath)
        {

            List<string> imageUrl = new List<string>();

            if (dto.ImageFile != null)
            {
                var uploadedUrls = await _fileStorage.UploadFileAsync(dto.ImageFile, "garmentServices", webRootPath);

                imageUrl.Add(uploadedUrls);
            }

            if (id <= 0)
                throw new ArgumentException("ID de GarmentService debe ser mayor a 0.");

            var domain = MapToDomain(dto);
            domain.ImageUrl = imageUrl;
            await _repo.PartialUpdateGarmentServiceAsync(id, domain);
        }

        public async Task DeleteGarmentServiceAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID debe ser mayor a 0.");

            await _repo.DeleteGarmentServiceAsync(id);
        }
    }
}
