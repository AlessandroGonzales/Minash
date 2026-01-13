using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class CustomRequest
    {
        public int IdCustom { get; set; }
        public string? SelectedColor { get; set; } 
        public string? SelectedSize { get; set; } 
        public List<IFormFile> ImageUrl { get; set; } = null!;
        public int? IdGarment { get; set; }
        public int? IdService { get; set; }
        public int? IdGarmentService { get; set; }
        public decimal CustomTotal { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public string CustomerDetails { get; set; } = null!;
    }
}
