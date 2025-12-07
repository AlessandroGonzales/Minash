using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Request
{
    public class RoleRequest
    {
        public int IdRole { get; set; }

        [Required(ErrorMessage = "El nombre del rol es obligatorio")]
        public string RoleName { get; set; } = null!;

        [Required(ErrorMessage ="Los detalles del rol son obligarios")]
        public string RoleDetails { get; set; } = null!;
    }
}
