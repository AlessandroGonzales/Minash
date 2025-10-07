using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class CustomRequest
    {
        public int IdCustom { get; set; }

        [Required]
        public string CustomerDetails { get; set; } = null!;

        [Required]
        [Range(1, (double)decimal.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Count { get; set; }
        public string ImageUrl { get; set; } = null!;

        [Required]
        public int IdUser { get; set; }
        [Required]
        public int IdGarment { get; set; }
        [Required]
        public int IdService { get; set; }
    }
}
