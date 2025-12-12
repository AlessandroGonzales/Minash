using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class AccountingRecordAppService : IAccountingRecordAppService
    {
        private readonly IAccountingRecordRepository _repoAccountingRecord;
        private readonly IPaymentRepository _repoPayment;
        public AccountingRecordAppService(
            IAccountingRecordRepository repoAccountingRecord,
            IPaymentRepository repoPayment
        )
        {
            _repoAccountingRecord = repoAccountingRecord;
            _repoPayment = repoPayment;
        }

        private static AccountingRecordResponse MapToResponse(AccountingRecord accoutingRecord) => new AccountingRecordResponse
        {
            IdAccountingRecord = accoutingRecord.IdAccountingRecord,
            IdPay = accoutingRecord.IdPay,
            Details = accoutingRecord.Details,
            Total = accoutingRecord.Total,
        };

        private static AccountingRecord MapToDomain(AccountingRecordRequest accountingRecord) => new AccountingRecord
        {
            IdAccountingRecord = accountingRecord.IdAccountingRecord,
            Details = accountingRecord.Details,
            Total = accountingRecord.total,
            IdPay = accountingRecord.idPay,
        };

        private static AccountingRecord MapToDomain(AccountingRecordsPartial accountingRecord) => new AccountingRecord
        {
            Total = accountingRecord.total,
            Details = accountingRecord.Details
        };

        public async Task<IEnumerable<AccountingRecordResponse>> GetAllAccountingRecordsAsync()
        {
            var list = await _repoAccountingRecord.GetAllAccountingRecordsAsync();
            return list.Select(MapToResponse);
        }

        public async Task<decimal> GetTotalByAccountingRecordAsync()
        {
            var total = await _repoAccountingRecord.GetTotalAccountingRecordAsync();
            var NewTotal = total.Sum(s => s.Total);
            return NewTotal;
        }

        public async Task <AccountingRecordResponse?> GetAccountingRecordByIdAsync(int id)
        {
            var aR = await _repoAccountingRecord.GetAccountingRecordByIdAsync(id);
            return aR == null ? null : MapToResponse(aR);
        }

        public async Task <AccountingRecordResponse> AddAccountingRecordAsync(AccountingRecordRequest accountingRecord)
        {
            if (accountingRecord == null)
                throw new ArgumentNullException(nameof(accountingRecord));
            var payment = await _repoPayment.GetPaymentByIdAsync(accountingRecord.idPay);
            if (payment == null)
                throw new ArgumentException($"Payment with ID {accountingRecord.idPay} not found.");

            var total = payment.Total;

            var newAccountingRecord = new AccountingRecord
            {
                IdAccountingRecord = accountingRecord.IdAccountingRecord,
                IdPay = accountingRecord.idPay,
                Details = accountingRecord.Details,
                Total = total,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var created = await _repoAccountingRecord.AddAccountingRecordAsync(newAccountingRecord);

            return MapToResponse(created);
        }

        public async Task UpdateAccountingRecordAsync(int id, AccountingRecordRequest accountingRecord)
        {
            var domain = MapToDomain(accountingRecord);

            await _repoAccountingRecord.UpdateAccountingRecordAsync(id, domain);
        }

        public async Task PartialUpdateAccountingRecordAsync(int id, AccountingRecordsPartial accountingRecordsDto)
        {
            var domain = MapToDomain(accountingRecordsDto);
            await _repoAccountingRecord.PartialUpdateAccountingRecordAsync(id, domain);
        }

        public async Task DeleteAccountingRecordAsync( int id)
        {
            await _repoAccountingRecord.DeleteAccountingRecordAsync(id);
        }
    }
}
