using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class GarmentServiceRequest
    {
        public int IdGarmentService { get; set; }
        [Required(ErrorMessage = "")]


        public decimal AdditionalPrice { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int IdGarment { get; set; }
        public int IdService { get; set; }
    }
}
