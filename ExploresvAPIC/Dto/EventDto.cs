namespace ExploresvAPIC.Dto
{
    public record EventDto
    (
        int Id,
        string Title,
        string Description,
        DateTime Date,
        List<ImageDto> Images //
    );
}