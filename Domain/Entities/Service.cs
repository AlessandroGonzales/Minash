namespace Domain.Entities
{
    public class Service
    {
        public int IdService { get; set; }
        public string ServiceName { get; set; } = null!;
        public string ServiceDetails { get; set; } = null!;
        public decimal Price { get; set; }
        public int state { get; set; }
        public string img_url { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set;  }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
