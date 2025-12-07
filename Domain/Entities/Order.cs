namespace Domain.Entities
{
    public class Order
    {
        // Attributes
        public int IdOrder { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Foreign Keys
        public int IdUser { get; set; }
        public User User { get; set; } = null!;
        public int? IdCustom { get; set; }
        public Custom? Custom { get; set; }

        // Navigation Properties
        public ICollection<DetailsOrder> DetailsOrders { get; set; } = new List<DetailsOrder>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
