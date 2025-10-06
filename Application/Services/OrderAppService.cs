using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class OrderAppService : IOrderAppService
    {
        private readonly IOrderRepository _repo;
        public OrderAppService(IOrderRepository repo)
        {
            _repo = repo;
        }

        private static OrderResponse MapToResponse(Order d) => new OrderResponse
        {
            IdOrder = d.IdOrder,
            TotalPrice = d.Total,
            IdUser = d.IdUser,
        };

        private static Order MapToDomain(OrderRequest dto) => new Order
        {
            IdOrder = dto.IdOrder,
            Total = dto.Total,
            IdUser = dto.IdUser,
        };

        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            var list = await _repo.GetAllOrdersAsync();
            return list.Select(MapToResponse);
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(int id)
        {
            var domain = await _repo.GetOrderByIdAsync(id);
            return domain != null ? MapToResponse(domain) : null;
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync(int userId)
        {
            var domainList = await _repo.GetOrdersByUserIdAsync(userId);
            return domainList.Select(MapToResponse);
        }

        public async Task<OrderResponse> AddOrderAsync(OrderRequest orderDto)
        {
            if (orderDto == null) throw new ArgumentNullException(nameof(orderDto));
            var order = MapToDomain(orderDto);

            var createdOrder = await _repo.AddOrderAsync(order);
            return MapToResponse(createdOrder);
        }

        public async Task UpdateOrderAsync(OrderRequest orderDto)
        {
            if (orderDto == null) throw new ArgumentNullException(nameof(orderDto));
            var order = MapToDomain(orderDto);

            await _repo.UpdateOrderAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _repo.DeleteOrderAsync(id);
        }
    }
}
