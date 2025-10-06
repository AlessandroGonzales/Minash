using Application.DTO.Request;
using Application.DTO.Response;

namespace Application.Interfaces
{
    public interface IAccountingRecordAppService
    {
        Task<IEnumerable<AccountingRecordResponse>> GetAllAccountingRecordsAsync();
        Task<AccountingRecordResponse> GetAccountingRecordByIdAsync(int id);
        Task<AccountingRecordResponse> AddAccountingRecordAsync(AccountingRecordRequest accountingRecordDto);
        Task UpdateAccountingRecordAsync(AccountingRecordRequest accountingRecordDto);
        Task DeleteAccountingRecordAsync(int id);
    }
}
