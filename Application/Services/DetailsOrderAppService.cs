using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class DetailsOrderAppService : IDetailsOrderAppService
    {
        private readonly IDetailsOrderRepository _repo;
        public DetailsOrderAppService(IDetailsOrderRepository repo)
        {
            _repo = repo;
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
            var list = await _repo.GetAllDetailsOrdersAsync();
            return list.Select(MapToResponse);
        }

        public async Task<DetailsOrderResponse?> GetDetailsOrderByIdAsync(int id)
        {
            var domain = await _repo.GetDetailsOrderByIdAsync(id);
            return domain != null ? MapToResponse(domain) : null;
        }

        public async Task<IEnumerable<DetailsOrderResponse>> GetDetailsOrdersByOrderIdAsync(int orderId)
        {
            var domainList = await _repo.GetDetailsOrdersByOrderIdAsync(orderId);
            return domainList.Select(MapToResponse);
        }

        public async Task<DetailsOrderResponse> AddDetailsOrderAsync(DetailsOrderRequest dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            var domain = MapToDomain(dto);
            var createdDomain = await _repo.AddDetailsOrderAsync(domain);
            return MapToResponse(createdDomain);
        }
        public async Task UpdateDetailsOrderAsync(int id, DetailsOrderRequest dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            var domain = MapToDomain(dto);
            await _repo.UpdateDetailsOrderAsync(id, domain);
        }

        public async Task DeleteDetailsOrderAsync (int id)
        {
            await _repo.DeleteDetailsOrderAsync(id);
        }
    }
}
