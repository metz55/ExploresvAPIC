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
        string CategoryName,
        int DepartmentId,
        string DepartmentName,
        int StatusId,
        string StatusName,
        List<ImageDto> Images,
        List<EventDto> Events 

    );
}