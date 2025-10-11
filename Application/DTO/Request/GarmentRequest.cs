using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class GarmentRequest
    {
        public int IdGarment { get; set; }

        [Required(ErrorMessage = "El nombre de la prenda es obligatorio")]
        public string GarmentName { get; set; } = null!;

        [Required(ErrorMessage = "Los detalles son obligatorios")]
        public string GarmentDetails { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }
}
