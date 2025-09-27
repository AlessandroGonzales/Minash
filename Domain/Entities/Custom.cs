namespace Domain.Entities
{
    public class Custom
    {
        public int IdCustom { get; set; }
        public string CustomerDetails { get; set; } = null!;
        public int Count { get; set; }
        public string ImageUrl { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        // Foreign Keys
        public int IdGarment { get; set; }
        public Garment Garment { get; set; } = null!;
        public int IdUser { get; set; }
        public User User { get; set; } = null!;
        public int IdService { get; set; }
        public Service Service { get; set; } = null!;

    }
}
