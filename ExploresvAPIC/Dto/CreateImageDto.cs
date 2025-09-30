namespace ExploresvAPIC.Dto
{
    public record CreateImageDto
    (
        byte[] Datos,
        int? EventId,
        int? TouristDestinationId
    );
}