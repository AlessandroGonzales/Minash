using Application.DTO.Partial;
using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IAccountingRecordAppService
    {
        Task<IEnumerable<AccountingRecordResponse>> GetAllAccountingRecordsAsync();
        Task<decimal> GetTotalByAccountingRecordAsync();
        Task<AccountingRecordResponse> GetAccountingRecordByIdAsync(int id);
        Task<AccountingRecordResponse> AddAccountingRecordAsync(AccountingRecordRequest accountingRecordDto);
        Task UpdateAccountingRecordAsync(int id, AccountingRecordRequest accountingRecordDto);
        Task PartialUpdateAccountingRecordAsync(int id, AccountingRecordsPartial accountingRecordDto);
        Task DeleteAccountingRecordAsync(int id);
    }
}
