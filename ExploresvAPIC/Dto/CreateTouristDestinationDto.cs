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
        List<string>? Images // Lista de bytes en lugar de IFormFile
        


    );
}