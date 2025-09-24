namespace ExploresvAPIC.Dto
{
    public record TouristDestinationDto
    (
        int Id,
        string Title,
        string Description,
        string Location,
        string Hours,
        int CategoryId,
        int DepartmentId,
        int StatusId,
        List<ImageDto> Images,
        int EventId
    );
}