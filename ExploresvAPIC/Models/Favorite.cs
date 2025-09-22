namespace ExploresvAPIC.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int TouristDestinationId { get; set; }
        public TouristDestination? TouristDestination { get; set; }
    }
}
