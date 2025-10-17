namespace Application.DTO.Response
{
    public class UserResponse
    {
        public int IdUser { get; set; }
        public int IdRol { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
        public string? Province { get; set; }
        public string? City { get; set; }
        public string? FullAddress { get; set; }

    }
}
