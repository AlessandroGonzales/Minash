using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class AccountingRecordAppService : IAccountingRecordAppService
    {
        private readonly IAccountingRecordRepository _repo;
        public AccountingRecordAppService(IAccountingRecordRepository repo) { _repo = repo; }

        private static AccountingRecordDto MapToDto(AccountingRecord accoutingRecord) => new AccountingRecordDto
        {
            IdAccountingRecord = accoutingRecord.IdAccountingRecord,
            idPay = accoutingRecord.IdPay,
            Details = accoutingRecord.Details,
            total = accoutingRecord.Total,
        };

        private static AccountingRecord MapToDomain(AccountingRecordDto accountingRecord) => new AccountingRecord
        {
            IdAccountingRecord = accountingRecord.IdAccountingRecord,
            Details = accountingRecord.Details,
            Total = accountingRecord.total,
            IdPay = accountingRecord.idPay,
        };

        public async Task<IEnumerable<AccountingRecordDto>> GetAllAccountingRecordsAsync()
        {
            var list = await _repo.GetAllAccountingRecordsAsync();
            return list.Select(MapToDto);
        }

        public async Task <AccountingRecordDto?> GetAccountingRecordByIdAsync(int id)
        {
            var aR = await _repo.GetAccountingRecordByIdAsync(id);
            return aR == null ? null : MapToDto(aR);
        }

        public async Task <AccountingRecordDto> AddAccountingRecordAsync(AccountingRecordDto accountingRecord)
        {
            var domain = MapToDomain(accountingRecord);
            domain.CreatedAt = DateTime.UtcNow;
            domain.UpdatedAt = DateTime.UtcNow;
            var created = await _repo.AddAccoutingRecordAsync(domain);
            return MapToDto(created);
        }

        public async Task UpdateAccountingRecordAsync( AccountingRecordDto accountingRecord)
        {
            var domain = MapToDomain(accountingRecord);
            domain.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAccountingRecordAsync(domain);
        }

        public async Task DeleteAccountingRecordAsync( int id)
        {
            await _repo.DeleteAccountingRecordAsync(id);
        }
    }
}
