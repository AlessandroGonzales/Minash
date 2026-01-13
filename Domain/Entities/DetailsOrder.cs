namespace Domain.Entities
{
    public class DetailsOrder
    {
        public int IdDetailsOrder { get; set; }
        public int Count { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public int IdOrder { get; set; }
        public string? SelectedColor { get; set; }
        public string? Details { get; set; }
        public string? SelectedSize { get; set; }
        public string? ServiceName { get; set; }
        public string? ImageUrl { get; set; }
        public Order Order { get; set; } = null!;
        public int? IdGarmentService { get; set; }
        public GarmentService? GarmentService { get; set; }
        public int? IdService { get; set; }
        public Service? Service { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
