using Domain.Entities;

namespace Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<Order>> GetOrdersByUserNameAsync(string userName);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<Order> AddOrderAsync(Order order);
        Task<Order> AddCustomOrderAsync(Order order);
        Task UpdateOrderAsync(int id, Order order);
        Task PartialUpdateOrderAsync(int id, Order order);
        Task DeleteOrderAsync(int id);
    }
}
