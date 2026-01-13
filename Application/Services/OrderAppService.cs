using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Infrastructure.ExternalServices;

namespace Application.Services
{
    public class OrderAppService : IOrderAppService
    {
        private readonly IOrderRepository _repo;
        private readonly GmailClient _gmailClient;
        public OrderAppService(IOrderRepository repo, GmailClient gmailClient)
        {
            _repo = repo;
            _gmailClient = gmailClient;
        }

        private static OrderResponse MapToResponse(Order d) => new OrderResponse
        {
            IdOrder = d.IdOrder,
            Total = d.Total,
            IdUser = d.IdUser,
            State = d.State,
        };

        private static Order MapToDomain(OrderRequest dto) => new Order
        {
            IdOrder = dto.IdOrder,
            Total = dto.Total,
            IdUser = dto.IdUser,
            State = dto.State,
        };  

        private static Order MapToDomain(OrderPartial dto) => new Order
        {
            Total = dto.Total,
            State = dto.State,
        };

        public async Task<IEnumerable<OrderResponse>> GetAllOrdersAsync()
        {
            var list = await _repo.GetAllOrdersAsync();
            return list.Select(MapToResponse);
        }

        public async Task<IEnumerable<Order>> GetAllPaidOrdersAsync()
        {
            var list =  await _repo.GetAllPaidOrdersAsync();
            return list;
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

        public async Task<Order> GetDraftOrderByUserIdAsync(int userId)
        {
            var draft = await _repo.GetDraftOrderByUserIdAsync(userId);

            if (draft != null)
            {
                return draft;
            }

            var newOrder = new Order
            {
                IdUser = userId,
                Total = 1,
                State = OrderState.Draft,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var created = await _repo.AddOrderAsync(newOrder);
            if (created.IdOrder <= 0)
            {
                throw new InvalidOperationException("No se pudo crear la orden draft - IdOrder inválido");
            }
            return created;
        }

        public async Task<IEnumerable<Order>> GetPaidOrderByUserIdAsync(int userId)
        {
            var paidOrder = await _repo.GetPaidOrderByUserIdAsync(userId);
            return paidOrder;
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
            var existingOrder = await _repo.GetOrderByIdAsync(id);
            if (existingOrder == null) return;

            var order = MapToDomain(orderDto);

            await _repo.PartialUpdateOrderAsync(id, order);

            if (orderDto.State.ToString() == "Completed")
            {
                string customerEmail = existingOrder.User?.Email ?? "";
                string customerName = existingOrder.User?.UserName ?? "Cliente";
                string orderIdStr = id.ToString().PadLeft(4, '0');

                if (!string.IsNullOrEmpty(customerEmail))
                {
                    _ = _gmailClient.SendOrderCompletedNotificationAsync(customerEmail, customerName, orderIdStr);
                }
            }

        }

        public async Task DeleteOrderAsync(int id)
        {
            await _repo.DeleteOrderAsync(id);
        }
    }
}
