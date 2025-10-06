namespace Application.DTO.Request
{
    public class RoleRequest
    {
        public int IdRole { get; set; }
        public string RoleName { get; set; } = null!;
        public string RoleDetails { get; set; } = null!;
    }
}
