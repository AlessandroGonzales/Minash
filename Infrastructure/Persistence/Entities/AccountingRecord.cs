namespace Infrastructure.Persistence.Entities;

public partial class AccountingRecord
{
    public int IdAccountingRecord { get; set; }

    public DateOnly EntryDate { get; set; }

    public decimal Total { get; set; }

    public string? Details { get; set; }

    public int IdPay { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Payment IdPayNavigation { get; set; } = null!;
}
