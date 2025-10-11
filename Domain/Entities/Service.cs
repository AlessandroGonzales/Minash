namespace Domain.Entities
{
    public class Service
    {
        // Attributes 
        public int IdService { get; set; }
        public string ServiceName { get; set; } = null!;
        public string ServiceDetails { get; set; } = null!;
        public decimal Price { get; set; }
        public int State { get; set; }
        public string ImageUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set;  }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public ICollection<Custom> Customs { get; set; } = new List<Custom>();
        public ICollection<GarmentService> GarmentServices { get; set; } = new List<GarmentService>();
    }
}
