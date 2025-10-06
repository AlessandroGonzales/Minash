using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using EfOrder = Infrastructure.Persistence.Entities.Order;
using EfPayment = Infrastructure.Persistence.Entities.Payment;
namespace Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly MinashDbContext _db;
        public PaymentRepository(MinashDbContext db)
        {
            _db = db;
        }

        private static Order MapToDomainOrder(EfOrder efOrder)
        {
            if (efOrder == null) return null!;
            return new Order
            {
                IdOrder = efOrder.IdOrder,
                Total = efOrder.Total,
                CreatedAt = efOrder.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = efOrder.UpdatedAt ?? DateTime.UtcNow,
                IdUser = efOrder.IdUser
            };
        }

        private static Payment MapToDomain(EfPayment ef) => new Payment
        {
            IdPay = ef.IdPay,
            Currency = ef.Currency,
            TransactionCode = ef.TransactionCode,
            ExternalPaymentId = ef.ExternalPaymentId,
            IdempotencyKey = ef.IdempotencyKey,
            PaymentMethod = ef.PaymentMethod,
            Provider = ef.Provider,
            ProviderResponse = string.IsNullOrWhiteSpace(ef.ProviderResponse)
            ? new Dictionary<string, object>()
            : JsonSerializer.Deserialize<Dictionary<string, object>>(ef.ProviderResponse),
            ReceiptImageUrl = ef.ReceiptImageUrl ?? string.Empty,
            Total = ef.Total,
            Verified = ef.Verified,
            Installments = ef.Installments,
            CreatedAt = ef.CreatedAt ?? DateTime.UtcNow,
            UpdatedAt = ef.UpdatedAt ?? DateTime.UtcNow,
            IdOrder = ef.IdOrder,
            Order = MapToDomainOrder(ef.IdOrderNavigation),

            Accountingrecords = ef.AccountingRecords?.Select(ar => new AccountingRecord
            {
                IdAccountingRecord = ar.IdAccountingRecord,
                Total = ar.Total,
                Details = ar.Details ?? string.Empty,
                CreatedAt = ar.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = ar.UpdatedAt ?? DateTime.UtcNow,
                IdPay = ar.IdPay,
            }).ToList() ?? new List<AccountingRecord>()
        };

        private static EfPayment MapToEf(Payment d) => new EfPayment
        {
            Currency = d.Currency,
            IdempotencyKey = d.IdempotencyKey,
            TransactionCode = d.TransactionCode,
            ExternalPaymentId = d.ExternalPaymentId,
            Provider = d.Provider,
            PaymentMethod = d.PaymentMethod,
            ProviderResponse = d.ProviderResponse == null
            ? "{}"
            : JsonSerializer.Serialize(d.ProviderResponse),
            ReceiptImageUrl = d.ReceiptImageUrl,
            Total = d.Total,
            Verified = d.Verified,
            Installments = d.Installments,
            CreatedAt = d.CreatedAt,
            UpdatedAt = d.UpdatedAt,
            IdOrder = d.IdOrder,
        };

        private IQueryable<EfPayment> GetQueryableWithIncludes(bool tracking = false)
        {
            var query = _db.Payments
                .Include(gs => gs.IdOrderNavigation);

            return tracking ? query : query.AsNoTracking();
          
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            var list = await GetQueryableWithIncludes().ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int IdOrder )
        {
            var list = await GetQueryableWithIncludes().Where(s => s.IdOrder == IdOrder).ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task<Payment?> GetPaymentByIdAsync(int Id)
        {
            var pay = await GetQueryableWithIncludes().FirstOrDefaultAsync(s => s.IdPay == Id);

            return pay == null ? null : MapToDomain(pay);

        }
     
        public async Task<Payment> AddPaymentAsync (Payment payment)
        {
            var creatPayment = MapToEf(payment);
            _db.Payments.Add(creatPayment);
            await _db.SaveChangesAsync();

            var createdPayment = await GetQueryableWithIncludes().FirstAsync(s => s.IdPay == creatPayment.IdPay);
            return MapToDomain(createdPayment);

        }
        public async Task UpdatePaymentAsync (Payment payment)
        {
            var updatePayment = await _db.Payments.FindAsync(payment.IdPay);
            if (updatePayment == null)
                throw new KeyNotFoundException();

            payment.Total = updatePayment.Total;
            payment.UpdatedAt = DateTime.Now;
            payment.Installments = updatePayment.Installments;
            payment.Currency = updatePayment.Currency;
            payment.ExternalPaymentId = updatePayment.ExternalPaymentId;
            payment.Verified = updatePayment.Verified;
            payment.Provider = updatePayment.Provider;
            payment.ReceiptImageUrl = updatePayment.ReceiptImageUrl;
            payment.IdempotencyKey = updatePayment.IdempotencyKey;
            payment.TransactionCode = updatePayment.TransactionCode;
            
            await _db.SaveChangesAsync();
        }

        public async Task DeletePaymentAsync (int idPay)
        {
            var pay = await _db.Payments.FindAsync(idPay);
            if (pay == null) throw new KeyNotFoundException();

            _db.Payments.Remove(pay);
            await _db.SaveChangesAsync();
        }

    }
}
