using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using EfGarmentService = Infrastructure.Persistence.Entities.GarmentService;
using EfOrder = Infrastructure.Persistence.Entities.Order;
using EfDetailsOrder = Infrastructure.Persistence.Entities.DetailsOrder;

namespace Infrastructure.Repositories
{
    public class DetailsOrderRepository : IDetailsOrderRepository
    {
        private readonly MinashDbContext _db;
        public DetailsOrderRepository(MinashDbContext db)
        {
            _db = db;
        }

        private static GarmentService MapToDomainGarmentService(EfGarmentService ef) => new GarmentService
        {
            IdGarmentService = ef.IdGarmentService,
            AdditionalPrice = ef.AdditionalPrice,
            ImageUrl = ef.ImageUrl ?? string.Empty,
            CreatedAt = ef.CreatedAt ?? DateTime.UtcNow,
            UpdatedAt = ef.UpdatedAt ?? DateTime.UtcNow,
            IdGarment = ef.IdGarment,
            IdService = ef.IdService
        };

        private static Order MapToDomainOrder(EfOrder ef) => new Order
        {
            IdOrder = ef.IdOrder,
            Total = ef.Total,
            CreatedAt = ef.CreatedAt ?? DateTime.UtcNow,
            UpdatedAt = ef.UpdatedAt ?? DateTime.UtcNow,
            IdUser = ef.IdUser
        };

        private static DetailsOrder MapToDomain(EfDetailsOrder ef) => new DetailsOrder
        {
            IdDetailsOrder = ef.IdDetailsOrder,
            Count = ef.Count,
            SubTotal = ef.SubTotal,
            UnitPrice = ef.UnitPrice,
            CreatedAt = ef.CreatedAt ?? DateTime.UtcNow,
            UpdatedAt = ef.UpdatedAt ?? DateTime.UtcNow,
            IdOrder = ef.IdOrder,
            Order = MapToDomainOrder(ef.IdOrderNavigation),
            IdGarmentService = ef.IdGarmentService,
            GarmentService = MapToDomainGarmentService(ef.IdGarmentServiceNavigation)
        };

        private static EfDetailsOrder MapToEf(DetailsOrder d) => new EfDetailsOrder
        {
            Count = d.Count,
            SubTotal = d.SubTotal,
            UnitPrice = d.UnitPrice,
            CreatedAt = d.CreatedAt,
            UpdatedAt = d.UpdatedAt,
            IdOrder = d.IdOrder,
            IdGarmentService = d.IdGarmentService
        };

        private IQueryable<EfDetailsOrder> GetQueryableWithIncludes(bool tracking = false)
        {
            var query = _db.DetailsOrders
                .Include(doe => doe.IdOrderNavigation)
                .Include(doe => doe.IdGarmentServiceNavigation);
            return tracking ? query : query.AsNoTracking();
        }


        public async Task<IEnumerable<DetailsOrder>> GetAllDetailsOrdersAsync()
        {
            var list = await GetQueryableWithIncludes().ToListAsync();
            return list.Select(MapToDomain);
        }
        public async Task<DetailsOrder?> GetDetailsOrderByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id must be greater than zero.");
            var efDetailsOrder = await GetQueryableWithIncludes().FirstOrDefaultAsync(doe => doe.IdDetailsOrder == id);
            return efDetailsOrder == null ? null : MapToDomain(efDetailsOrder);
        }

        public async Task<IEnumerable<DetailsOrder>> GetDetailsOrdersByOrderIdAsync(int orderId)
        {
            if (orderId <= 0)
                throw new ArgumentException("OrderId must be greater than zero.");
            var list = await GetQueryableWithIncludes()
                .Where(doe => doe.IdOrder == orderId)
                .ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task<DetailsOrder> AddDetailsOrderAsync(DetailsOrder d)
        {
            var efDetailsOrder = MapToEf(d);
            _db.DetailsOrders.Add(efDetailsOrder);
            await _db.SaveChangesAsync();
            
            var created = await GetQueryableWithIncludes().FirstOrDefaultAsync(doe => doe.IdDetailsOrder == efDetailsOrder.IdDetailsOrder);
            return MapToDomain(created);
        }

        public async Task UpdateDetailsOrderAsync(int id, DetailsOrder d)
        {
            if (d.IdDetailsOrder <= 0)
                throw new ArgumentException("DetailsOrder must have a valid IdDetailsOrder greater than zero to be updated.");

            var existingEf = await _db.DetailsOrders.FindAsync(id);
            if (existingEf == null)
                throw new InvalidOperationException($"DetailsOrder with Id {d.IdDetailsOrder} does not exist.");

            existingEf.Count = d.Count;
            existingEf.SubTotal = d.SubTotal;
            existingEf.UnitPrice = d.UnitPrice;
            existingEf.UpdatedAt = DateTime.UtcNow;

            _db.DetailsOrders.Update(existingEf);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteDetailsOrderAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id must be greater than zero.");
            var existingEf = await _db.DetailsOrders.FindAsync(id);
            if (existingEf == null)
                throw new InvalidOperationException($"DetailsOrder with Id {id} does not exist.");
            _db.DetailsOrders.Remove(existingEf);
            await _db.SaveChangesAsync();
        }
    }
}
