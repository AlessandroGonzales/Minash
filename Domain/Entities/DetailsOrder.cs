namespace Domain.Entities
{
    public class DetailsOrder
    {
        public int IdDetailsOrder { get; set; }
        public int Count { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public int IdOrder { get; set; }
        public Order Order { get; set; } = null!;
        public int IdGarmentService { get; set; }
        public GarmentService GarmentService { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
