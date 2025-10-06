using Domain.Entities;

namespace Domain.Repositories
{
    public interface IAccountingRecordRepository
    {
        Task<IEnumerable<AccountingRecord>> GetAllAccountingRecordsAsync();
        Task<AccountingRecord?> GetAccountingRecordByIdAsync(int id);
        Task<AccountingRecord> AddAccoutingRecordAsync(AccountingRecord accountingrecord);
        Task UpdateAccountingRecordAsync( AccountingRecord accountingrecord);
        Task DeleteAccountingRecordAsync(int id); 
    }
}
