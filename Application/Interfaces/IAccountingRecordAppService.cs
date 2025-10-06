using Application.DTO;

namespace Application.Interfaces
{
    public interface IAccountingRecordAppService
    {
        Task<IEnumerable<AccountingRecordDto>> GetAllAccountingRecordsAsync();
        Task<AccountingRecordDto> GetAccountingRecordByIdAsync(int id);
        Task<AccountingRecordDto> AddAccountingRecordAsync(AccountingRecordDto accountingRecordDto);
        Task UpdateAccountingRecordAsync(AccountingRecordDto accountingRecordDto);
        Task DeleteAccountingRecordAsync(int id);
    }
}
