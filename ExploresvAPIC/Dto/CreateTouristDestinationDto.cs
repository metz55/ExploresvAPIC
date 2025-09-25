namespace ExploresvAPIC.Dto
{
    public record CreateTouristDestinationDto
    (
        string Title,
        string Description,
        string Location,
        string Hours,
        int CategoryId,
        int DepartmentId,
        string DepartmentName,
        int StatusId,
        string StatusName,
        List<ImageDto> Images,
        int EventId


    );
}