using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using EfUser = Infrastructure.Persistence.Entities.User;
using EfOrder = Infrastructure.Persistence.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
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
                Address = ef.Address,
                Email = ef.Email,
                Phone = ef.Phone,
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
            State = ef.State,

            DetailsOrders = ef.DetailsOrders.Select(doe => new DetailsOrder
            {
                IdDetailsOrder = doe.IdDetailsOrder,
                Count = doe.Count,
                SubTotal = doe.SubTotal,
                UnitPrice = doe.UnitPrice,
                SelectedColor = doe.SelectedColor,
                ServiceName = doe.ServiceName,
                ImageUrl = doe.ImageUrl,
                SelectedSize = doe.SelectedSize,
                Details = doe.Details ?? string.Empty,
                CreatedAt = doe.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = doe.UpdatedAt ?? DateTime.UtcNow,
                IdOrder = doe.IdOrder,
                IdGarmentService = doe.IdGarmentService,
                IdService = doe.IdService,
                
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
                PaymentMethod = p.PaymentMethod ?? string.Empty,
                UpdatedAt = p.UpdatedAt ?? DateTime.UtcNow,
                IdOrder = p.IdOrder
            }).ToList() ?? new List<Payment>(),
            
            Customs = ef.Customs?.Select(c => new Custom
            {
                IdCustom = c.IdCustom,
                SelectedColor = c.SelectedColor ?? string.Empty,
                SelectedSize = c.SelectedSize ?? string.Empty,
                IdGarmentService = c.IdGarmentService,
                IdService = c.IdService,
                Count = c.Count,
                ImageUrl = c.ImageUrl ?? new List<string>(),
                IdUser = c.IdUser,
                IdGarment = c.IdGarment,
                CustomerDetails = c.CustomerDetails,
                CreatedAt = c.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = c.UpdatedAt ?? DateTime.UtcNow,
                IdOrder = c.IdOrder ?? 0,
                CustomName = c.CustomName ?? string.Empty,
                CustomTotal = c.CustomTotal ?? 0,
                UnitPrice = c.UnitPrice,
            }).ToList() ?? new List<Custom>(),


        };

        private static EfOrder MapToEf(Order order) => new EfOrder
        {
            Total = order.Total,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            IdUser = order.IdUser,
            State = order.State,

        };

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            var list = await _db.Orders
                .Include(o => o.IdUserNavigation)
                .AsNoTracking()
                .ToListAsync();

            return list.Select(MapToDomain);
        }

        public async Task<Order?> GetDraftOrderByUserIdAsync(int userId)
        {
            var ef = await _db.Orders
            .Where(o => o.IdUser == userId && o.State == OrderState.Draft)
            .Include(o => o.IdUserNavigation)
            .Include(o => o.DetailsOrders)
            .Include(o => o.Customs)
            .Include(o => o.Payments)
            .OrderByDescending(o => o.CreatedAt)
            .AsNoTracking()
            .FirstOrDefaultAsync();

            return ef == null ? null : MapToDomain(ef);
        }

        public async Task<IEnumerable<Order>> GetPaidOrderByUserIdAsync(int userId)
        {
            var list = await _db.Orders
            .Where(o => o.IdUser == userId && o.State == OrderState.Paid)
            .Include(o => o.IdUserNavigation)
            .Include(o => o.DetailsOrders)
            .Include(o => o.Customs)
            .Include(o => o.Payments)
            .OrderByDescending(o => o.CreatedAt)
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

        public async Task<IEnumerable<Order>> GetAllPaidOrdersAsync()
        {
            var list = await _db.Orders
                .Where(o => o.State == OrderState.Paid)
                .Include(o => o.IdUserNavigation)
                .Include(o => o.DetailsOrders)
                .Include(o => o.Customs)
                .Include(o => o.Payments)
                .OrderByDescending(o => o.CreatedAt)
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
            efOrder.CreatedAt = DateTime.UtcNow;
            efOrder.UpdatedAt = DateTime.UtcNow;

            await _db.Orders.AddAsync(efOrder);
            await _db.SaveChangesAsync();

            return MapToDomain(efOrder);
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

            if (order.Total != 0) efOrder.Total = order.Total;

            if (order.State != default) efOrder.State = order.State;

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
