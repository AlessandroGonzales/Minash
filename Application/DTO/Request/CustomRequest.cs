using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class CustomRequest
    {
        public int IdCustom { get; set; }

        [Required]
        public string CustomerDetails { get; set; } = null!;

        [Required]
        public int Count { get; set; }

        public List<IFormFile> ImageUrl { get; set; } = null!;

        [Required]
        public int IdUser { get; set; }
        public int? IdGarment { get; set; }
        public int? IdService { get; set; }

        public int? IdGarmentService { get; set; }
    }
}
