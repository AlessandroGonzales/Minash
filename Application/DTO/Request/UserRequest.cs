namespace Application.DTO.Request
{
    public class UserRequest
    {
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
        public int IdRole { get; set; }

    }
}
