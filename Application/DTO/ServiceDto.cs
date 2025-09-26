namespace Application.DTO
{
    public class ServiceDto
    {
        public int IdService { get; set; }
        public string ServiceName { get; set; } = null!;
        public string ServiceDetails { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}

