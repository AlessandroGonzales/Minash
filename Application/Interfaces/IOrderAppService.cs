using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IOrderAppService 
    {
        Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
        Task<OrderResponse?> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync(int userId);
        Task<OrderResponse> AddOrderAsync(OrderRequest orderDto);
        Task UpdateOrderAsync(OrderRequest orderDto);
        Task DeleteOrderAsync(int id);
    }
}
