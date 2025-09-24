namespace ExploresvAPIC.Dto
{
    public record CreateTouristDestinationDto
    (
        string Title,
        string Description,
        string Location,
        string Hours,
        List<ImageDto> Images
    );
}