using Domain.Entities;

namespace Domain.Repositories
{
    public interface IAccountingRecordRepository
    {
        Task<IEnumerable<AccountingRecord>> GetAllAccountingRecordsAsync();
        Task<IEnumerable<AccountingRecord>> GetTotalAccountingRecordAsync();
        Task<AccountingRecord?> GetAccountingRecordByIdAsync(int id);
        Task<AccountingRecord> AddAccountingRecordAsync(AccountingRecord accountingrecord);
        Task UpdateAccountingRecordAsync(int id, AccountingRecord accountingrecord);
        Task PartialUpdateAccountingRecordAsync(int id, AccountingRecord record);
        Task DeleteAccountingRecordAsync(int id); 
    }
}
