namespace Domain.Entities
{
    public class Role
    {
        public int IdRole { get; set; }
        public string RoleName { get; set; } = null!;
        public string RoleDetails { get; set; } = null!;    
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }     

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
