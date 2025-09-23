namespace ExploresvAPIC.Dto
{
    public record eventDto
    (
        int Id,
        string Title,
        string Description,
        DateTime Date,
        int Images
    );
}