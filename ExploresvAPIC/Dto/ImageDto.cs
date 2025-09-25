using ExploresvAPIC.Models;

namespace ExploresvAPIC.Dto
{
    public record ImageDto
    (
        int id,
        byte[] Datos,
        int EventId,
        int? TouristDestinationId
    );
}