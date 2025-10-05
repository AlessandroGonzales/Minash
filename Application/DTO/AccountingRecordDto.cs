namespace Application.DTO
{
    public class AccountingRecordDto
    {
        public int IdAccountingRecord {  get; set; }
        public decimal total {  get; set; }
        public string? Details { get; set; }
        public int idPay { get; set; }
    }
}
