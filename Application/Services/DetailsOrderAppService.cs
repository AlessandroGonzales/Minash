using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;  
using Domain.Repositories;

namespace Application.Services
{
    public class DetailsOrderAppService : IDetailsOrderAppService
    {
        private readonly IDetailsOrderRepository _repoDetailsOrder;
        private readonly IPaymentRepository _repoPayment;
        private readonly IOrderRepository _repoOrder;
        private readonly IGarmentServiceRepository _repoGarmenService;
        private readonly IServiceRepository _serviceRepository;
        public DetailsOrderAppService(
            IDetailsOrderRepository repoDetailOrder,
            IPaymentRepository repoPayment,
            IOrderRepository repoOrder,
            IGarmentServiceRepository repoGarmenService
            IServiceRepository serviceRepository
            )
        {
            _repoDetailsOrder = repoDetailOrder;
            _repoPayment = repoPayment;
            _repoOrder = repoOrder;
            _repoGarmenService = repoGarmenService;
            _serviceRepository = serviceRepository;
        }

        private static DetailsOrderResponse MapToResponse(DetailsOrder d) => new DetailsOrderResponse
        {
            IdDetailsOrder = d.IdDetailsOrder,
            IdOrder = d.IdOrder,
            IdGarmentService = d.IdGarmentService,
            UnitPrice = d.UnitPrice,
            SubTotal = d.SubTotal,
            Count = d.Count,
            selectColor = d.SelectedColor,
            selectSize = d.SelectedSize,
            IdService = d.IdService,
            details = d.Details
        };

        private static DetailsOrder MapToDomain(DetailsOrderRequest dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new DetailsOrder
            {
                IdDetailsOrder = dto.IdDetailsOrder,
                IdGarmentService = dto.IdGarmentService,
                IdOrder = dto.IdOrder,
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

        public async Task<DetailsOrderResponse> AddDetailsOrderAsync(DetailsOrderRequest dto)
        {
            
            var order = await _repoOrder.GetOrderByIdAsync(dto.IdOrder)
                ?? throw new Exception($"Order with ID {dto.IdOrder} not found.");

            if((dto.IdGarmentService.HasValue && dto.IdService.HasValue) || (!dto.IdGarmentService.HasValue && !dto.IdService.HasValue))
            {
                throw new Exception("You must provide either IdGarmentService OR IdService.");
            }

            decimal unitPrice;

            if(dto.IdGarmentService.HasValue)
            {
                var garmentService = await _repoGarmenService.GetGarmentServiceByIdAsync(dto.IdGarmentService.Value)
                ?? throw new Exception($"GarmentService {dto.IdGarmentService} not found.");

                unitPrice = garmentService.AdditionalPrice;
            }
            else
            {
                var service = await _serviceRepository
                    .GetServiceByIdAsync(dto.IdService!.Value)
                    ?? throw new Exception($"Service {dto.IdService} not found.");

                unitPrice = service.Price;
            }

            var detailOrder = new DetailsOrder
            {
                IdOrder = dto.IdOrder,
                IdGarmentService = dto.IdGarmentService,
                IdService = dto.IdService,
                Count = dto.Count,
                UnitPrice = unitPrice,
                SubTotal = unitPrice * dto.Count,
                SelectedColor = dto.SelectedColor,
                SelectedSize = dto.SelectedSize,
                Details = dto.Details,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdDetail = await _repoDetailsOrder.AddDetailsOrderAsync(detailOrder);
            var details = await _repoDetailsOrder.GetDetailsOrdersByOrderIdAsync(order.IdOrder);
            order.Total = details.Sum(d => d.SubTotal);
            order.UpdatedAt = DateTime.UtcNow;

            await _repoOrder.PartialUpdateOrderAsync(order.IdOrder, order);

            return MapToResponse(createdDetail);
        }
        public async Task UpdateDetailsOrderAsync(int id, DetailsOrderRequest dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            var domain = MapToDomain(dto);
            await _repoDetailsOrder.UpdateDetailsOrderAsync(id, domain);
        }

        public async Task DeleteDetailsOrderAsync (int id)
        {
            await _repoDetailsOrder.DeleteDetailsOrderAsync(id);
        }
    }
}
