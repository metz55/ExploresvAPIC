namespace ExploresvAPIC.Dto
{
    public record CreateEventDto
    (
        string Title,
        string Description,
        DateTimeOffset Date,
        int TouristDestinationId, 
        List<byte[]> Images
    );
}