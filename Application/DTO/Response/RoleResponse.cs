namespace Application.DTO.Response
{
    public class RoleResponse
    {
        public int IdRol { get; set; }
        public string RoleName { get; set; } = null!;
        public string RoleDetails{ get; set; } = string.Empty;
    }
}
