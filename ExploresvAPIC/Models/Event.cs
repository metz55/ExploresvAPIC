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
        public DateTime Date { get; set; } = default!;
        public List<Image> Images { get; set; } = new();
    }
}
