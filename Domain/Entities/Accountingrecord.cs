namespace Domain.Entities
{
    public class Accountingrecord
    {
        public int IdAccountingRecord { get; set; }
        public decimal Total { get; set; }
        public string Details { get; set; } = null!;
        public int IdPay { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
