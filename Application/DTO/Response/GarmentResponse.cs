namespace Application.DTO.Response
{
    public class GarmentResponse
    {
        public int IdGarment { get; set; }
        public string GarmentName { get; set; } = null!;
        public string GarmentDetails { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public List<string> Colors { get; set; } = null!;
        public List<string> Sizes { get; set; } = null!;
    }
}
