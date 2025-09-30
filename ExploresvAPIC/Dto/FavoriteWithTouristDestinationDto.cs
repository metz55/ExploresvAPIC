namespace ExploresvAPIC.Dto
{
    public record FavoriteWithTouristDestinationDto
    (
        int Id,
        int UserId,
        int TouristDestinationId,
        TouristDestinationDto TouristDestination //Incluye Dto completo de TouristDestination
    );
}