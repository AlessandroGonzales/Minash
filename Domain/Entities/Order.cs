using Domain.Enums;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Order
    {
        // Attributes
        public int IdOrder { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderState State { get; set; }

        // Foreign Keys
        public int IdUser { get; set; }
        public User User { get; set; } = null!;

        // Navigation Properties
        public ICollection<DetailsOrder> DetailsOrders { get; set; } = new List<DetailsOrder>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public ICollection<Custom> Customs { get; set; } = new List<Custom>();
    }
}
