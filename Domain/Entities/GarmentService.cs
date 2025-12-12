namespace Domain.Entities
{
    public class GarmentService
    {
        // Attributes
        public int IdGarmentService { get; set; }
        public decimal AdditionalPrice { get; set; }
        public List<string>? ImageUrl { get; set; }
        public string GarmentServiceName { get; set; } = null!;
        public List<string> Colors { get; set; } = new List<string>();
        public List<string> Sizes { get; set; } = new List<string>();
        public string GarmentServiceDetails { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Foreign Keys
        public int IdGarment { get; set; }
        public Garment Garment { get; set; } = null!;
        public int IdService { get; set; }
        public Service Service { get; set; } = null!;

        // Navigation Properties
        public ICollection<DetailsOrder> DetailsOrders { get; set; } = new List<DetailsOrder>();
    }
}
