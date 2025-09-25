namespace ExploresvAPIC.Dto
{
    public record UserDto
    (
        int Id,
        string Name,
        string Apellido,
        string Email,
        string Clave,
        int RoleId,
        string RoleName
    );
}