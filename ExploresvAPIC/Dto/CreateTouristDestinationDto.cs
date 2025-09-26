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
        int StatusId,
        List<IFormFile>? Images,
        int? EventId // Opcional


    );
}