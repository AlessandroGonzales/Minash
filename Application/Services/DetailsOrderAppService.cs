
using Application.DTO;
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

        private static DetailsOrderDto MapToDto(DetailsOrder d) => new DetailsOrderDto
        {
            IdDetailsOrder = d.IdDetailsOrder,
            IdGarmentService = d.IdGarmentService,
            IdOrder = d.IdOrder,
            UnitPrice = d.UnitPrice,
            Subtotal = d.SubTotal,
            Count = d.Count,
        };

        private static DetailsOrder MapToDomain(DetailsOrderDto dto)
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

        public async Task<IEnumerable<DetailsOrderDto>> GetAllDetailsOrdersAsync()
        {
            var list = await _repo.GetAllDetailsOrdersAsync();
            return list.Select(MapToDto);
        }

        public async Task<DetailsOrderDto?> GetDetailsOrderByIdAsync(int id)
        {
            var domain = await _repo.GetDetailsOrderByIdAsync(id);
            return domain != null ? MapToDto(domain) : null;
        }

        public async Task<IEnumerable<DetailsOrderDto>> GetDetailsOrdersByOrderIdAsync(int orderId)
        {
            var domainList = await _repo.GetDetailsOrdersByOrderIdAsync(orderId);
            return domainList.Select(MapToDto);
        }

        public async Task<DetailsOrderDto> AddDetailsOrderAsync(DetailsOrderDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            var domain = MapToDomain(dto);
            var createdDomain = await _repo.AddDetailsOrderAsync(domain);
            return MapToDto(createdDomain);
        }
        public async Task UpdateDetailsOrderAsync(DetailsOrderDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            var domain = MapToDomain(dto);
            await _repo.UpdateDetailsOrderAsync(domain);
        }

        public async Task DeleteDetailsOrderAsync (int id)
        {
            await _repo.DeleteDetailsOrderAsync(id);
        }
    }
}
