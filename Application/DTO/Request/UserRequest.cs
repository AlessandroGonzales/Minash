using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class UserRequest
    {
        public int IdUser { get; set; }
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Phone { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public IFormFile ImageUrl { get; set; } = null!;
        public string Province { get; set; } = null!;
        public string City { get; set; } = null!;
        public string FullAddress { get; set; } = null!;
        public int IdRole { get; set; } = 1;

    }
}
