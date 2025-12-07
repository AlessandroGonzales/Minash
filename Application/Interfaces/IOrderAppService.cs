using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IOrderAppService 
    {
        Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
        Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<OrderResponse>> GetOrdersByUserNameAsync(string userName);
        Task<OrderResponse?> GetOrderByIdAsync(int id);
        Task<OrderResponse> AddOrderAsync(OrderRequest orderDto);
        Task UpdateOrderAsync(int id, OrderRequest orderDto);
        Task PartialUpdateOrderAsync(int id, OrderPartial orderDto);
        Task DeleteOrderAsync(int id);
    }
}
