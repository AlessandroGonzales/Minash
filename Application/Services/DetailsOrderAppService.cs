using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;  
using Domain.Repositories;

namespace Application.Services
{
    public class DetailsOrderAppService : IDetailsOrderAppService
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
            { "Bordado en Remeras",  new PricingRule { Threshold = 10, DiscountPercentage = 0.20m }}
        };

        private readonly IDetailsOrderRepository _repoDetailsOrder;
        private readonly IPaymentRepository _repoPayment;
        private readonly IOrderRepository _repoOrder;
        private readonly IGarmentServiceRepository _repoGarmenService;
        private readonly IServiceRepository _serviceRepository;
        private readonly IOrderAppService _orderAppService;
        private readonly ICustomRepository _customRepo;

        public DetailsOrderAppService(
            IDetailsOrderRepository repoDetailOrder,
            IPaymentRepository repoPayment,
            IOrderRepository repoOrder,
            IGarmentServiceRepository repoGarmenService,
            IServiceRepository serviceRepository,
            IOrderAppService orderAppService,
            ICustomRepository customRepo
            )
        {
            _repoDetailsOrder = repoDetailOrder;
            _repoPayment = repoPayment;
            _repoOrder = repoOrder;
            _repoGarmenService = repoGarmenService;
            _serviceRepository = serviceRepository;
            _orderAppService = orderAppService;
            _customRepo = customRepo;
        }
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

        private static DetailsOrderResponse MapToResponse(DetailsOrder d) => new DetailsOrderResponse
        {
            IdDetailsOrder = d.IdDetailsOrder,
            IdOrder = d.IdOrder,
            IdGarmentService = d.IdGarmentService,
            UnitPrice = d.UnitPrice,
            SubTotal = d.SubTotal,
            Count = d.Count,
            SelectColor = d.SelectedColor,
            SelectSize = d.SelectedSize,
            IdService = d.IdService,
            Details = d.Details,
            ImageUrl = d.ImageUrl,
            ServiceName = d.ServiceName 
        };

        private static DetailsOrder MapToDomain(DetailsOrderRequest dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new DetailsOrder
            {
                IdDetailsOrder = dto.IdDetailsOrder,
                IdGarmentService = dto.IdGarmentService,
                UnitPrice = dto.UnitPrice,
                SubTotal = dto.Subtotal,
                Count = dto.Count,
                SelectedColor = dto.SelectedColor,
                SelectedSize = dto.SelectedSize,
                IdService = dto.IdService,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
        }
        

        public async Task<IEnumerable<DetailsOrderResponse>> GetAllDetailsOrdersAsync()
        {
            var list = await _repoDetailsOrder.GetAllDetailsOrdersAsync();
            return list.Select(MapToResponse);
        }

        public async Task<DetailsOrderResponse?> GetDetailsOrderByIdAsync(int id)
        {
            var domain = await _repoDetailsOrder.GetDetailsOrderByIdAsync(id);
            return domain != null ? MapToResponse(domain) : null;
        }

        public async Task<IEnumerable<DetailsOrderResponse>> GetDetailsOrdersByOrderIdAsync(int orderId)
        {
            var domainList = await _repoDetailsOrder.GetDetailsOrdersByOrderIdAsync(orderId);
            return domainList.Select(MapToResponse);
        }

        public async Task<DetailsOrderResponse> AddDetailsOrderAsync(int userId, DetailsOrderRequest dto)
        {
            var order = await _orderAppService.GetDraftOrderByUserIdAsync(userId);

            if (order.State != Domain.Enums.OrderState.Draft)
            {
                throw new InvalidOperationException(
                $"Cannot modify order {order.IdOrder} because it is in state {order.State}."
                );
            }

            if((dto.IdGarmentService.HasValue && dto.IdService.HasValue) || (!dto.IdGarmentService.HasValue && !dto.IdService.HasValue))
            {
                throw new Exception("You must provide either IdGarmentService OR IdService.");
            }

            decimal unitPrice;
            string nameService;
            string? imageUrl = null; 

            if (dto.IdGarmentService.HasValue)
            {
                var garmentService = await _repoGarmenService.GetGarmentServiceByIdAsync(dto.IdGarmentService.Value)
                ?? throw new Exception($"GarmentService {dto.IdGarmentService} not found.");

                nameService = garmentService.GarmentServiceName;
                imageUrl = garmentService.ImageUrl?.FirstOrDefault();

                unitPrice = garmentService.AdditionalPrice;
            }
            else
            {
                var service = await _serviceRepository
                    .GetServiceByIdAsync(dto.IdService!.Value)
                    ?? throw new Exception($"Service {dto.IdService} not found.");
                
                nameService = service.ServiceName;
                imageUrl = service.ImageUrl;
                unitPrice = service.Price;
            }

            decimal finalSubTotal = CalculateSubTotal(nameService, dto.Count, unitPrice);

            var detailOrder = new DetailsOrder
            {
                IdOrder = order.IdOrder,
                IdGarmentService = dto.IdGarmentService,
                IdService = dto.IdService,
                Count = dto.Count,
                UnitPrice = unitPrice,
                SubTotal = finalSubTotal,
                SelectedColor = dto.SelectedColor,
                SelectedSize = dto.SelectedSize,
                Details = dto.Details,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ServiceName = nameService,
                ImageUrl = imageUrl
            };

            var createdDetail = await _repoDetailsOrder.AddDetailsOrderAsync(detailOrder);
            var details = await _repoDetailsOrder.GetDetailsOrdersByOrderIdAsync(createdDetail.IdOrder);
            var allCustoms = await _customRepo.GetCustomsByOrderIdAsync(createdDetail.IdOrder);

            decimal detailsSubtotal = details.Sum(d => d.SubTotal);
            decimal customsSubtotal = allCustoms.Sum(c => c.Count * c.UnitPrice); 

            order.Total = detailsSubtotal + customsSubtotal;

            await _repoOrder.PartialUpdateOrderAsync(createdDetail.IdOrder, order);

            return MapToResponse(createdDetail);
        }

        public async Task UpdateDetailsOrderAsync(int id, DetailsOrderRequest dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            var domain = MapToDomain(dto);
            await _repoDetailsOrder.UpdateDetailsOrderAsync(id, domain);
        }

        public async Task DeleteDetailsOrderAsync(int id)
        {
            var detail = await _repoDetailsOrder.GetDetailsOrderByIdAsync(id)
                ?? throw new Exception($"DetailsOrder {id} not found.");

            var order = await _repoOrder.GetOrderByIdAsync(detail.IdOrder)
            ?? throw new Exception($"Order {detail.IdOrder} not found.");

            if (order.State != Domain.Enums.OrderState.Draft)
            {
                throw new InvalidOperationException(
                    $"Cannot modify order {order.IdOrder} because it is in state {order.State}."
                );
            }

            await _repoDetailsOrder.DeleteDetailsOrderAsync(id);

            order.Total -= detail.SubTotal;

            await _repoOrder.PartialUpdateOrderAsync(detail.IdOrder, order);
        }


    }
}
