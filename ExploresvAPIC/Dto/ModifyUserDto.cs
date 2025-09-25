namespace ExploresvAPIC.Dto
{
    public record ModifyUserDto
    (
        int Id,
        string Name,
        string Apellido,
        string Email,
        string Clave,
        int RoleId
    );
}