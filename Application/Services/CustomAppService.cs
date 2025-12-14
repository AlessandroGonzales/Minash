using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class CustomAppService : ICustomAppService
    {
        private readonly ICustomRepository _repo;
        private readonly IOrderRepository _orderRepo;
        private readonly IGarmentRepository _garmentRepo;
        private readonly IServiceRepository _serviceRepo;
        private readonly IGarmentServiceRepository _garmentServiceRepo;
        private readonly IFileStorageService _fileStorage;
        public CustomAppService(ICustomRepository repo, IOrderRepository orderRepo, IGarmentRepository garmentRepo, IServiceRepository serviceRepo, IGarmentServiceRepository garmentServiceRepo, IFileStorageService fileStorage)
        {
            _repo = repo;
            _orderRepo = orderRepo;
            _garmentRepo = garmentRepo;
            _serviceRepo = serviceRepo;
            _garmentServiceRepo = garmentServiceRepo;
            _fileStorage = fileStorage;
        }
        private static CustomResponse MapToResponse(Custom custom) => new CustomResponse
        {
            IdCustom = custom.IdCustom,
            IdService = custom.IdService,
            IdGarment = custom.IdGarment,
            IdUser = custom.IdUser,
            CustomDetails = custom.CustomerDetails,
            Count = custom.Count,
            ImageUrl = custom.ImageUrl,
            SelectedColor = custom.SelectedColor,
            SelectedSize = custom.SelectedSize,
            IdGarmentService = custom.IdGarmentService
        };

        private static Custom MapToDomain(CustomRequest dto) => new Custom
        {
            IdCustom = dto.IdCustom,
            IdService = dto.IdService,
            SelectedSize = dto.SelectedSize,
            SelectedColor = dto.SelectedColor,
            IdGarment = dto.IdGarment,
            IdUser = dto.IdUser,
            CustomerDetails = dto.CustomerDetails,
            Count = dto.Count,
            IdGarmentService = dto.IdGarmentService,
            CreatedAt= DateTime.UtcNow,
            UpdatedAt= DateTime.UtcNow,
        };

        private static Custom MapToDomain(CustomPartial dto) => new Custom
        {
            Count = dto.count,
        };

        public async Task<IEnumerable<CustomResponse>> GetAllCustomsAsync()
        {
            var list = await _repo.GetAllCustomsAsync();
            return list.Select(MapToResponse);
        }
        public async Task<IEnumerable<CustomResponse>> GetCustomsByUserNameAsync(string userName)
        {
            var list = await _repo.GetCustomsByUserNameAsync(userName);
            return list.Select(MapToResponse);
        }
        public async Task<CustomResponse> GetCustomByIdAsync(int id)
        {

            var custom = await _repo.GetCustomByIdAsync(id);
            if (custom == null)
            {
                throw new KeyNotFoundException($"Custom with ID {id} not found.");
            }
            return MapToResponse(custom);
        }
        public async Task<CustomResponse> AddCustomAsync(CustomRequest custom, string webRootPath)
        {

            if (!custom.IdGarmentService.HasValue &&
                (!custom.IdGarment.HasValue || !custom.IdService.HasValue))
            {
                throw new ArgumentException(
                    "Debe enviar IdGarmentService o bien IdGarment e IdService."
                );
            }

            List<string> imageUrl = new List<string>();
            if(custom.ImageUrl != null && custom.ImageUrl.Count > 0)
            {   
                imageUrl = await _fileStorage.UploadFilesAsync(custom.ImageUrl, "custom", webRootPath);
            }

            Garment? garment = null;
            Service? service = null;
            GarmentService? garmentService = null;

            if (custom.IdGarmentService.HasValue)
            {
                garmentService = await _garmentServiceRepo
                    .GetGarmentServiceByIdAsync(custom.IdGarmentService.Value);

                if (garmentService == null)
                    throw new KeyNotFoundException("GarmentService no encontrada.");
            }
            else
            {
                garment = await _garmentRepo.GetGarmentByIdAsync(custom.IdGarment!.Value);
                service = await _serviceRepo.GetServiceByIdAsync(custom.IdService!.Value);

                if (garment == null || service == null)
                    throw new KeyNotFoundException("Garment o Service no encontrados.");
            }


            decimal total = garmentService != null
                ? custom.Count * garmentService.AdditionalPrice
                : custom.Count * (service!.Price + garment!.Price);


            var creatCustom = MapToDomain(custom);
            creatCustom.ImageUrl = imageUrl;
            var createdCustom = await _repo.AddCustomAsync(creatCustom);
            
            var order = new Order
            {
                IdCustom = createdCustom.IdCustom,
                IdUser = createdCustom.IdUser,
                Total = total,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _orderRepo.AddOrderAsync(order);
            return MapToResponse(createdCustom);

        }
        public async Task UpdateCustomAsync(int id, CustomRequest custom)
        {
            var UpdateCustom = MapToDomain(custom);
            await _repo.UpdateCustomAsync(id, UpdateCustom);
        }
        public async Task PartialUpdateCustomAsync(int id, CustomPartial custom)
        {
            var UpdateCustom = MapToDomain(custom);
            await _repo.PartialUpdateCustomAsync(id, UpdateCustom);
        }
        public async Task DeleteCustomAsync(int id)
        {
            await _repo.DeleteCustomAsync(id);
        }
    }
}
