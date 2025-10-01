using Application.DTO;
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

        private static OrderDto MapToDto(Order d) => new OrderDto
        {
            IdOrder = d.IdOrder,
            Total = d.Total,
            IdUser = d.IdUser,
        };

        private static Order MapToDomain(OrderDto dto) => new Order
        {
            IdOrder = dto.IdOrder,
            Total = dto.Total,
            IdUser = dto.IdUser,
        };

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var list = await _repo.GetAllOrdersAsync();
            return list.Select(MapToDto);
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            var domain = await _repo.GetOrderByIdAsync(id);
            return domain != null ? MapToDto(domain) : null;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var domainList = await _repo.GetOrdersByUserIdAsync(userId);
            return domainList.Select(MapToDto);
        }

        public async Task<OrderDto> AddOrderAsync(OrderDto orderDto)
        {
            if (orderDto == null) throw new ArgumentNullException(nameof(orderDto));
            var order = MapToDomain(orderDto);
            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
            var createdOrder = await _repo.AddOrderAsync(order);
            return MapToDto(createdOrder);
        }

        public async Task UpdateOrderAsync(OrderDto orderDto)
        {
            if (orderDto == null) throw new ArgumentNullException(nameof(orderDto));
            var order = MapToDomain(orderDto);
            order.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateOrderAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _repo.DeleteOrderAsync(id);
        }
    }
}
