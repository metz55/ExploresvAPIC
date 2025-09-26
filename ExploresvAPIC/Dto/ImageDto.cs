namespace ExploresvAPIC.Dto
{
    public record ImageDto
    (
        int Id,
        byte[] Datos,
        int? EventId,
        int? TouristDestinationId
    );
}