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
        private struct PricingRule
        {
            public int Threshold { get; set; }
            public decimal DiscountPercentage { get; set; }
        }

        // Usamos StringComparer.OrdinalIgnoreCase para que "Bordado" sea igual a "bordado"
        private static readonly Dictionary<string, PricingRule> _pricingRules = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Estampado", new PricingRule { Threshold = 10, DiscountPercentage = 0.20m } }, // 20%
            { "Bordado",   new PricingRule { Threshold = 10, DiscountPercentage = 0.25m } }, // 25%
            { "DTF",       new PricingRule { Threshold = 10, DiscountPercentage = 0.15m } }, // 15%
        };

        private readonly ICustomRepository _repo;
        private readonly IOrderRepository _orderRepo;
        private readonly IGarmentRepository _garmentRepo;
        private readonly IServiceRepository _serviceRepo;
        private readonly IGarmentServiceRepository _garmentServiceRepo;
        private readonly IFileStorageService _fileStorage;
        private readonly IOrderAppService _orderAppService;
        private readonly IDetailsOrderRepository _detailsRepo;

        private decimal CalculateSubTotal(string? serviceName, int count, decimal unitPrice)
        {
            if (string.IsNullOrWhiteSpace(serviceName)) return unitPrice * count;

            string normalizedSearch = serviceName.Trim();

            if (_pricingRules.TryGetValue(normalizedSearch, out var rule))
            {
                if (count >= rule.Threshold)
                {
                    decimal multiplier = 1m - rule.DiscountPercentage;
                    return (unitPrice * count) * multiplier;
                }
            }

            return unitPrice * count;
        }

        public CustomAppService(ICustomRepository repo, IOrderRepository orderRepo,IDetailsOrderRepository detailsrepo ,IGarmentRepository garmentRepo, IOrderAppService orderService,IServiceRepository serviceRepo, IGarmentServiceRepository garmentServiceRepo, IFileStorageService fileStorage)
        {
            _repo = repo;
            _orderRepo = orderRepo;
            _garmentRepo = garmentRepo;
            _serviceRepo = serviceRepo;
            _garmentServiceRepo = garmentServiceRepo;
            _fileStorage = fileStorage;
            _orderAppService = orderService;
            _detailsRepo = detailsrepo;
        }
        private static CustomResponse MapToResponse(Custom custom) => new CustomResponse
        {
            IdCustom = custom.IdCustom,
            IdService = custom.IdService,
            IdGarment = custom.IdGarment,
            IdUser = custom.IdUser,
            CustomerDetails = custom.CustomerDetails,
            Count = custom.Count,
            ImageUrl = custom.ImageUrl,
            SelectedColor = custom.SelectedColor,
            SelectedSize = custom.SelectedSize,
            IdGarmentService = custom.IdGarmentService,
            IdOrder = custom.IdOrder,
            UnitPrice = custom.UnitPrice,
            CustomTotal = custom.CustomTotal,
            CustomerName = custom.CustomName,
        };

        private static Custom MapToDomain(CustomRequest dto) => new Custom
        {
            IdCustom = dto.IdCustom,
            IdService = dto.IdService,
            SelectedSize = dto.SelectedSize,
            SelectedColor = dto.SelectedColor,
            IdGarment = dto.IdGarment,
            CustomerDetails = dto.CustomerDetails,
            Count = dto.Count,
            CustomTotal = dto.CustomTotal ,
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

        public async Task<IEnumerable<CustomResponse>> GetCustomsByOrderIdAsync(int orderId)
        {
            var list = await _repo.GetCustomsByOrderIdAsync(orderId);
            return list.Select(MapToResponse);
        }

        public async Task<IEnumerable<CustomResponse>> GetCustomsByUserNameAsync(string userName)
        {
            var list = await _repo.GetCustomsByUserNameAsync(userName);
            return list.Select(MapToResponse);
        }
        public async Task<CustomResponse?> GetCustomByIdAsync(int id)
        {

            var custom = await _repo.GetCustomByIdAsync(id);
            return custom == null ? null : MapToResponse(custom);
        }

        public async Task<CustomResponse> AddCustomAsync(int userId, CustomRequest custom, string webRootPath)
        {
            var order = await _orderAppService.GetDraftOrderByUserIdAsync(userId);
            if (order.State != Domain.Enums.OrderState.Draft)
            {
                throw new InvalidOperationException(
                $"Cannot modify order {order.IdOrder} because it is in state {order.State}."
                );
            }
            if (!custom.IdGarmentService.HasValue && (!custom.IdGarment.HasValue || !custom.IdService.HasValue))
            {
                throw new ArgumentException(
                    "Debe enviar IdGarmentService o bien (IdGarment e IdService) ambos con valor."
                );
            }

            List<string> imageUrl = new List<string>();
            if(custom.ImageUrl != null && custom.ImageUrl.Count > 0)
            {   
                imageUrl = await _fileStorage.UploadFilesAsync(custom.ImageUrl, "custom", webRootPath);
            }

            decimal unitPrice;
            string nameService;
            string? fullNameService = null;
            if (custom.IdGarmentService.HasValue)
            {
                var gs = await _garmentServiceRepo.GetGarmentServiceByIdAsync(custom.IdGarmentService.Value)
                    ?? throw new Exception("GarmentService no encontrada");

               unitPrice = gs.AdditionalPrice;
               nameService = gs.GarmentServiceName;
            }
            else
            { 
                var garment = await _garmentRepo.GetGarmentByIdAsync(custom.IdGarment!.Value)
                    ?? throw new Exception("Garment no encontrado");
                var service = await _serviceRepo.GetServiceByIdAsync(custom.IdService!.Value)
                    ?? throw new Exception("Service no encontrado");

                unitPrice = garment.Price + service.Price;
                nameService = service.ServiceName;
                fullNameService = service.ServiceName + " en " + garment.GarmentName;
            }

            decimal finalSubTotal = CalculateSubTotal( 
                nameService,
                custom.Count,
                unitPrice
            );

            var newCustom = MapToDomain(custom);
            newCustom.IdOrder = order.IdOrder;                 
            newCustom.ImageUrl = imageUrl;
            newCustom.UnitPrice = unitPrice;
            newCustom.IdUser = userId;
            newCustom.CustomTotal = finalSubTotal;
            newCustom.CustomName = fullNameService;


            if (custom.IdGarmentService.HasValue)
            {
                newCustom.IdGarment = null;
                newCustom.IdService = null;
            }
            else
            {
                newCustom.IdGarmentService = null;
            }

            var createdCustom = await _repo.AddCustomAsync(newCustom);

            var allCustoms = await _repo.GetCustomsByOrderIdAsync(order.IdOrder);
            var allDetails = await _detailsRepo.GetDetailsOrdersByOrderIdAsync(order.IdOrder);

            decimal customsSubTotal = allCustoms.Sum(c => c.CustomTotal);
            decimal detailsSubtotal = allDetails.Sum(d => d.SubTotal);

            order.Total = customsSubTotal + detailsSubtotal;

            await _orderRepo.PartialUpdateOrderAsync(order.IdOrder, order);


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
            var custom = await _repo.GetCustomByIdAsync(id)
                ?? throw new Exception($"Custom {id} not found.");

            var order = await _orderRepo.GetOrderByIdAsync(custom.IdOrder)
                ?? throw new Exception($"Order {id} not found.");

            await _repo.DeleteCustomAsync(id);

            order.Total -= custom.CustomTotal;

            await _orderRepo.PartialUpdateOrderAsync(order.IdOrder, order);

        }
    }
}
