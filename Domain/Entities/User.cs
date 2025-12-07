namespace Domain.Entities
{
    public class User
    {
        // Attributes
        public int IdUser { get; set; }
        public string UserName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string City { get; set; } = null!;
        public string FullAddress { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Foreign Keys
        public int IdRole { get; set; }
        public Role Role { get; set; } = null!;

        // Navigation Properties
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Custom> Customs { get; set; } = new List<Custom>();

    }
}
