namespace Application.DTO.Response
{
    public class AccountingRecordResponse
    {
        public int IdAccountingRecord { get; set; }
        public int IdPay {  get; set; }
        public decimal Total {  get; set; }
        public string Details { get; set; } = null!;

    }
}
