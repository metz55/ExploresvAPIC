namespace ExploresvAPIC.Models
{
    public class Image
    {
        public int Id { get; set; }
        public byte[] Datos { get; set; } = default!;
        public int? EventId { get; set; }
        public int TouristDestinationId { get; set; }
        public TouristDestination? TouristDestination { get; set; }
    }
}