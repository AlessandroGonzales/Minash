namespace Domain.Entities
{
    public class Garment
    {
        public int IdGarment { get; set; }
        public string GarmentName { get; set; } = null!;
        public string GarmentDetails { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public List<string> Colors { get; set; } = new List<string>();
        public List<string> Sizes { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal Price { get; set; }
        public ICollection<GarmentService> GarmentServices { get; set; } = new List<GarmentService>();
        public ICollection<Custom> Customs { get; set; } = new List<Custom>();
    }
}
