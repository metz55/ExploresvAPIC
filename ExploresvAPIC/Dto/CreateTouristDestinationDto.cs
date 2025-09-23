namespace ExploresvAPIC.Dto
{
    public record CreateTouristDestinationDto
    (
        int Id,
        string Title,
        string Description,
        string Location,
        string Hours,
        List<ImageDto> Images
    );
}