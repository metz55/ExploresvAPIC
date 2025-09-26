namespace ExploresvAPIC.Dto
{
    public record EventDto
    (
        int Id,
        string Title,
        string Description,
        DateTimeOffset Date,
        int TouristDestinationId,
        List<ImageDto> Images
    );
}