using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using EfAccountingRecord = Infrastructure.Persistence.Entities.AccountingRecord;
using EfPayment = Infrastructure.Persistence.Entities.Payment;
namespace Infrastructure.Repositories
{
    public class AccountingRecordRepository : IAccountingRecordRepository
    {
        private readonly MinashDbContext _db;
        public AccountingRecordRepository(MinashDbContext db) { _db = db; }

        private static Payment MapToDomainPayment(EfPayment payment) => new Payment
        {
            IdPay = payment.IdPay,
            IdOrder = payment.IdOrder,
            IdempotencyKey = payment.IdempotencyKey,
            Installments = payment.Installments,
            ExternalPaymentId = payment.ExternalPaymentId,
            Provider = payment.Provider,
            ReceiptImageUrl = payment.ReceiptImageUrl ?? string.Empty,
            Total = payment.Total,
            TransactionCode = payment.TransactionCode,
            Verified = payment.Verified,
            Currency = payment.Currency,
            ProviderResponse = string.IsNullOrWhiteSpace(payment.ProviderResponse)
            ? new Dictionary<string, object>()
            : JsonSerializer.Deserialize<Dictionary<string, object>>(payment.ProviderResponse),
            CreatedAt = payment.CreatedAt ?? DateTime.UtcNow,
            UpdatedAt = payment.UpdatedAt ?? DateTime.UtcNow,
        };

        private static AccountingRecord MapToDomain(EfAccountingRecord accountingRecord) => new AccountingRecord
        {
            IdAccountingRecord = accountingRecord.IdAccountingRecord,
            Details = accountingRecord.Details,
            Total= accountingRecord.Total,
            CreatedAt = accountingRecord?.CreatedAt ?? DateTime.UtcNow,
            UpdatedAt = accountingRecord?.UpdatedAt ?? DateTime.UtcNow,
            IdPay = accountingRecord.IdPay,
            Payment = MapToDomainPayment(accountingRecord.IdPayNavigation),
        };

        private static EfAccountingRecord MapToEf(AccountingRecord accountingrecord) => new EfAccountingRecord
        {
            IdAccountingRecord = accountingrecord.IdAccountingRecord,
            Details = accountingrecord.Details,
            Total = accountingrecord.Total,
            CreatedAt = accountingrecord.CreatedAt,
            UpdatedAt = accountingrecord.UpdatedAt,
            IdPay = accountingrecord.IdPay,

        };

        private IQueryable<EfAccountingRecord> GetQueryableWithIncludes(bool tracking = false)
        {
            var query = _db.AccountingRecords.Include(s =>  s.IdPayNavigation);
            return tracking ? query : query.AsNoTracking();
        }

        public async Task<IEnumerable<AccountingRecord>> GetAllAccountingRecordsAsync()
        {
            var list = await GetQueryableWithIncludes().ToListAsync();
            return list.Select(MapToDomain);
        }

        public async Task <AccountingRecord?> GetAccountingRecordByIdAsync(int id)
        {
            var accoutingRecord = await GetQueryableWithIncludes().FirstOrDefaultAsync(s => s.IdAccountingRecord == id);
            return accoutingRecord == null ? null : MapToDomain(accoutingRecord);
        }

        public async Task <AccountingRecord> AddAccoutingRecordAsync(AccountingRecord accoutingRecord)
        {
            var ef = MapToEf(accoutingRecord);
            _db.AccountingRecords.Add(ef);
            await _db.SaveChangesAsync();
            ef.CreatedAt = DateTime.UtcNow;
            ef.UpdatedAt = DateTime.UtcNow;
            var creatdEf = await GetQueryableWithIncludes().FirstOrDefaultAsync(s => s.IdPay == ef.IdAccountingRecord);
            return MapToDomain(creatdEf);
        }

        public async Task UpdateAccountingRecordAsync( AccountingRecord accountingRecord)
        {
            var ef = await _db.AccountingRecords.FindAsync(accountingRecord.IdAccountingRecord);
            ef.Details= accountingRecord.Details;
            ef.Total= accountingRecord.Total;
            ef.Total= accountingRecord.Total;
            ef.UpdatedAt = DateTime.UtcNow,
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAccountingRecordAsync( int id)
        {
            var aR = await _db.AccountingRecords.FindAsync(id);
            _db.AccountingRecords.Remove(aR);
            await _db.SaveChangesAsync();
        }
    }
}
