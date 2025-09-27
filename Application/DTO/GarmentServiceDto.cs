namespace Application.DTO
{
    public class GarmentServiceDto
    {
        public int IdGarmentService { get; set; }
        public decimal AdditionalPrice { get; set; }
        public string ImageUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int IdGarment { get; set; }
        public int IdService { get; set; }

    }
}
