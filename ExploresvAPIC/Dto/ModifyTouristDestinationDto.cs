namespace ExploresvAPIC.Dto
{
    public record ModifyTouristDestinationDto
    (
        int Id,
        string Title,
        string Description,
        string Location,
        string Hours
    );
}