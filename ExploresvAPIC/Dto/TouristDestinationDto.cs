namespace ExploresvAPIC.Dto
{
    public record touristDestinationDto
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