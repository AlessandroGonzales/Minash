using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services
{
    public class AccountingRecordAppService : IAccountingRecordAppService
    {
        private readonly IAccountingRecordRepository _repo;
        public AccountingRecordAppService(IAccountingRecordRepository repo) { _repo = repo; }

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

        public async Task<IEnumerable<AccountingRecordResponse>> GetAllAccountingRecordsAsync()
        {
            var list = await _repo.GetAllAccountingRecordsAsync();
            return list.Select(MapToResponse);
        }

        public async Task <AccountingRecordResponse?> GetAccountingRecordByIdAsync(int id)
        {
            var aR = await _repo.GetAccountingRecordByIdAsync(id);
            return aR == null ? null : MapToResponse(aR);
        }

        public async Task <AccountingRecordResponse> AddAccountingRecordAsync(AccountingRecordRequest accountingRecord)
        {
            var domain = MapToDomain(accountingRecord);
            var created = await _repo.AddAccoutingRecordAsync(domain);
            return MapToResponse(created);
        }

        public async Task UpdateAccountingRecordAsync( AccountingRecordRequest accountingRecord)
        {
            var domain = MapToDomain(accountingRecord);

            await _repo.UpdateAccountingRecordAsync(domain);
        }

        public async Task DeleteAccountingRecordAsync( int id)
        {
            await _repo.DeleteAccountingRecordAsync(id);
        }
    }
}
