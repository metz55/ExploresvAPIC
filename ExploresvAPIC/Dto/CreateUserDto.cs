namespace ExploresvAPIC.Dto
{
    public record CreateUserDto
    (
        string Name,
        string Apellido,
        string Email,
        string Clave
    );
}