using Domain.Enums;

namespace Infrastructure.Persistence.Entities;

public partial class Payment
{
    public PaymentMethod PaymentMethod { get; set; }
    public int IdPay { get; set; }

    public DateOnly PaymentDate { get; set; }

    public decimal Total { get; set; }

    public int IdOrder { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string Provider { get; set; } = null!;

    public string? ExternalPaymentId { get; set; }

    public string? TransactionCode { get; set; }

    public string? ProviderResponse { get; set; }

    public bool Verified { get; set; }

    public string Currency { get; set; } = null!;

    public int? Installments { get; set; }

    public string? ReceiptUrl { get; set; }

    public string? IdempotencyKey { get; set; }

    public string? ReceiptImageUrl { get; set; }

    public virtual ICollection<AccountingRecord> AccountingRecords { get; set; } = new List<AccountingRecord>();

    public virtual Order IdOrderNavigation { get; set; } = null!;
}
