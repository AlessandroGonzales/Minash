using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IOrderAppService 
    {
        Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();

        Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<OrderResponse>> GetOrdersByUserNameAsync(string userName);
        Task<IEnumerable<Order>> GetAllPaidOrdersAsync();
        Task<OrderResponse?> GetOrderByIdAsync(int id);
        Task<Order> GetDraftOrderByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GetPaidOrderByUserIdAsync(int userId);
        Task<OrderResponse> AddOrderAsync(OrderRequest orderDto);
        Task UpdateOrderAsync(int id, OrderRequest orderDto);
        Task PartialUpdateOrderAsync(int id, OrderPartial orderDto);
        Task DeleteOrderAsync(int id);
    }
}
