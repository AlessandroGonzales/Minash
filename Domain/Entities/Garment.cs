namespace Domain.Entities
{
    public class Garment
    {
        public int IdGarment { get; set; }
        public string GarmentName { get; set; } = null!;
        public string GarmentDetails { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
