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
        int Images,
        int EventId
    );
}