using System.ComponentModel.DataAnnotations;

namespace ExploresvAPIC.Models
{
    public class Event
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; } = default!;

        [MaxLength(2500)]
        public string Description { get; set; } = default!;

        public DateTimeOffset Date { get; set; }

        // Un evento pertenece a un destino
        public int TouristDestinationId { get; set; }
        public TouristDestination? TouristDestination { get; set; }

        // Un evento puede tener imágenes
        public List<Image> Images { get; set; } = new();
    }
}
