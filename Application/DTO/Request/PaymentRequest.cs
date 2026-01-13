namespace Application.DTO.Request{
    public class PaymentRequest
    {
        public int IdPay { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public decimal Total { get; set; }
        public string Provider { get; set; } = null!;
        public string? ExternalPaymentId { get; set; } 
        public string? TransactionCode { get; set; }
        public Dictionary<string, object>? ProviderResponse { get; set; }
        public bool Verified { get; set; }
        public string Currency { get; set; } = null!;
        public int? Installments { get; set; }
        public string ReceiptImageUrl { get; set; } = null!;
        public string? IdempotencyKey { get; set; }
        public int IdOrder { get; set; }
    }
}
