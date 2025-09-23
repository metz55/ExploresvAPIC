namespace ExploresvAPIC.Dto
{
    public record CreateEventDto
    (
        int Id,
        string Title,
        string Description,
        DateTime Date
    );
}