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
        public DetailsOrderAppService(
            IDetailsOrderRepository repoDetailOrder,
            IPaymentRepository repoPayment,
            IOrderRepository repoOrder,
            IGarmentServiceRepository repoGarmenService
            )
        {
            _repoDetailsOrder = repoDetailOrder;
            _repoPayment = repoPayment;
            _repoOrder = repoOrder;
            _repoGarmenService = repoGarmenService;
        }

        private static DetailsOrderResponse MapToResponse(DetailsOrder d) => new DetailsOrderResponse
        {
            IdDetailsOrder = d.IdDetailsOrder,
            IdOrder = d.IdOrder,
            IdGarmentService = d.IdGarmentService,
            UnitPrice = d.UnitPrice,
            SubTotal = d.SubTotal,
            Count = d.Count,
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

            var garmentService = await _repoGarmenService.GetGarmentServiceByIdAsync(dto.IdGarmentService);
            if (garmentService == null)
            {
                throw new Exception($"Garment Service with ID {dto.IdGarmentService} not found.");
            }

            var order = await _repoOrder.GetOrderByIdAsync(dto.IdOrder);
            if (order == null)
            {
                throw new Exception($"Order with ID {dto.IdOrder} not found.");
            }

            var unitPrice = garmentService.AdditionalPrice;
            var subTotal = dto.Count * unitPrice;

            var newDetailOrder = new DetailsOrder
            {
                IdOrder = dto.IdOrder,
                IdGarmentService = dto.IdGarmentService,
                Count = dto.Count,
                UnitPrice = unitPrice,
                SubTotal = subTotal,
                IdDetailsOrder = dto.IdDetailsOrder,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var createdDomain = await _repoDetailsOrder.AddDetailsOrderAsync(newDetailOrder);

            var details = await _repoDetailsOrder.GetDetailsOrdersByOrderIdAsync(dto.IdOrder);
            var newTotal = details.Sum(d => d.SubTotal);
            order.Total = newTotal;
            await _repoOrder.PartialUpdateOrderAsync(order.IdOrder, order);

            var payment = await _repoPayment.GetPaymentsByOrderIdAsync(dto.IdOrder);
            if (payment == null)
            {
                throw new Exception($"Payment for Order ID {dto.IdOrder} not found.");
            }
            payment.Total = newTotal;
            await _repoPayment.PartialUpdatePaymentAsync(payment.IdPay ,payment);

            return MapToResponse(createdDomain);
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
