using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IDetailsOrderAppService
    {
        Task<IEnumerable<DetailsOrderResponse>> GetAllDetailsOrdersAsync();
        Task<DetailsOrderResponse?> GetDetailsOrderByIdAsync(int id);
        Task<IEnumerable<DetailsOrderResponse>> GetDetailsOrdersByOrderIdAsync(int orderId);
        Task<DetailsOrderResponse> AddDetailsOrderAsync(DetailsOrderRequest detailsOrderDto);
        Task UpdateDetailsOrderAsync(DetailsOrderRequest detailsOrderDto);
        Task DeleteDetailsOrderAsync(int id);
    }
}
