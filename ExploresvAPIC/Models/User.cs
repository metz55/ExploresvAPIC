namespace ExploresvAPIC.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Apellido { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Clave { get; set; } = default!;
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public int StatusId { get; set; }
        public Status? Status { get; set; }
    }
}