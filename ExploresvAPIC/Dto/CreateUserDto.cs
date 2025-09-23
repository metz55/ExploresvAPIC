namespace ExploresvAPIC.Dto
{
    public record CreateUserDto
    (
        int Id,
        string Name,
        string Apellido,
        string Email,
        string Clave
    );
}