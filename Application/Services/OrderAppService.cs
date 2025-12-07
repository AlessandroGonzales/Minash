using Application.DTO.Partial;
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
            IdCustom = d.IdCustom,
        };

        private static Order MapToDomain(OrderRequest dto) => new Order
        {
            IdOrder = dto.IdOrder,
            Total = dto.Total,
            IdUser = dto.IdUser,
            IdCustom = dto.IdCustom,
        };

        private static Order MapToDomain(OrderPartial dto) => new Order
        {
            Total = dto.Total,
        };

        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            var list = await _repo.GetAllOrdersAsync();
            return list.Select(MapToResponse);
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersByUserNameAsync(string userName)
        {
            var list = await _repo.GetOrdersByUserNameAsync(userName);
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

        public async Task UpdateOrderAsync(int id, OrderRequest orderDto)
        {
            if (orderDto == null) throw new ArgumentNullException(nameof(orderDto));
            var order = MapToDomain(orderDto);

            await _repo.UpdateOrderAsync(id, order);
        }

        public async Task PartialUpdateOrderAsync(int id, OrderPartial orderDto)
        {
            var order = MapToDomain(orderDto);

            await _repo.PartialUpdateOrderAsync(id, order);

        }
        public async Task DeleteOrderAsync(int id)
        {
            await _repo.DeleteOrderAsync(id);
        }
    }
}
