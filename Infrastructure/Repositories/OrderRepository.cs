using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using EfUser = Infrastructure.Persistence.Entities.User;
using EfOrder = Infrastructure.Persistence.Entities.Order;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MinashDbContext _db;
        public OrderRepository(MinashDbContext db)
        {
            _db = db;
        }

        private static User MapToDomainUser (EfUser ef)
        {
            if (ef == null) return null!;
            return new User
            {
                IdUser = ef.IdUser,
                UserName = ef.UserName,
                LastName = ef.LastName,
                Email = ef.Email,
                PasswordHash = ef.PasswordHash,
                Phone = ef.Phone,
                Address = ef.Address,
                CreatedAt = ef.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = ef.UpdatedAt ?? DateTime.UtcNow,
                ImageUrl = ef.ImageUrl ?? string.Empty,
                Province = ef.Province ?? string.Empty,
                City = ef.City ?? string.Empty,
                FullAddress = ef.FullAddress ?? string.Empty,
                IdRole = ef.IdRole,
            };
        }

        private static Order MapToDomain(EfOrder ef) => new Order
        {
            IdOrder = ef.IdOrder,
            Total = ef.Total,
            CreatedAt = ef.CreatedAt ?? DateTime.UtcNow,
            UpdatedAt = ef.UpdatedAt ?? DateTime.UtcNow,
            IdUser = ef.IdUser,
            User = MapToDomainUser(ef.IdUserNavigation),

            DetailsOrders = ef.DetailsOrders.Select(doe => new DetailsOrder
            {
                IdDetailsOrder = doe.IdDetailsOrder,
                Count = doe.Count,
                SubTotal = doe.SubTotal,
                UnitPrice = doe.UnitPrice,
                CreatedAt = doe.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = doe.UpdatedAt ?? DateTime.UtcNow,
                IdOrder = doe.IdOrder,
                IdGarmentService = doe.IdGarmentService
            }).ToList(),

            Payments = ef.Payments?.Select(p => new Payment
            {
                IdPay = p.IdPay,
                Total = p.Total,
                Currency = p.Currency,
                ReceiptImageUrl = p.ReceiptImageUrl ?? string.Empty,
                ExternalPaymentId = p.ExternalPaymentId ?? string.Empty,
                Provider = p.Provider ?? string.Empty,
                Installments = p.Installments,
                Verified = p.Verified,
                TransactionCode = p.TransactionCode ?? string.Empty,
                CreatedAt = p.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = p.UpdatedAt ?? DateTime.UtcNow,
                IdOrder = p.IdOrder
            }).ToList() ?? new List<Payment>()
        };

        private static EfOrder MapToEf(Order order) => new EfOrder
        {
            Total = order.Total,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            IdUser = order.IdUser,
        };

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var list = await _db.Orders
                .Include(o => o.IdUserNavigation)
                .AsNoTracking()
                .ToListAsync();

            return list.Select(MapToDomain);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserNameAsync(string userName)
        {
            var list = await _db.Orders
                .Include(o => o.IdUserNavigation)
                .Where(o => o.IdUserNavigation.UserName == userName)
                .AsNoTracking()
                .ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            var ef = await _db.Orders
                .Include(o => o.IdUserNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.IdOrder == id);
            return ef == null ? null : MapToDomain(ef);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            var list = await _db.Orders
                .Where(o => o.IdUser == userId)
                .Include(o => o.IdUserNavigation)
                .AsNoTracking()
                .ToListAsync();
            return list.Select(MapToDomain);
        }
        public async Task<Order> AddOrderAsync(Order order)
        {
            var efOrder = MapToEf(order);
            await _db.Orders.AddAsync(efOrder);
            await _db.SaveChangesAsync();
            order.CreatedAt = DateTime.Now;
            order.UpdatedAt = DateTime.Now;
            order.IdOrder = efOrder.IdOrder;
            return order;
        }

        public async Task UpdateOrderAsync(int id, Order order)
        {
            var efOrder = await _db.Orders.FindAsync(id);
            if (efOrder == null) throw new KeyNotFoundException($"Order with ID {id} not found.");

            efOrder.Total = order.Total;
            efOrder.UpdatedAt = DateTime.UtcNow;

            _db.Orders.Update(efOrder);
            await _db.SaveChangesAsync();
        }

        public async Task PartialUpdateOrderAsync(int id, Order order)
        {
            var efOrder = await _db.Orders.FindAsync(id);
            if (efOrder == null) throw new KeyNotFoundException($"Order with ID {id} not found.");

            efOrder.Total = order.Total;
            _db.Orders.Update(efOrder);
            await _db.SaveChangesAsync();

        }
        public async Task DeleteOrderAsync(int id)
        {
            var efOrder = await _db.Orders.FindAsync(id);
            if (efOrder == null) throw new KeyNotFoundException($"Order with ID {id} not found.");
            _db.Orders.Remove(efOrder);
            await _db.SaveChangesAsync();
        }
    }
}
