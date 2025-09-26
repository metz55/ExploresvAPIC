using System.ComponentModel.DataAnnotations;

namespace ExploresvAPIC.Models
{
    public class TouristDestination
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; } = default!;
        [MaxLength(2500)]
        public string Description { get; set; } = default!;
        [MaxLength(100)]
        public string Location { get; set; } = default!;
        public string Hours { get; set; } = default!;
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int DepartmentId { get; set; }

        public Department? Department { get; set; }
        public int StatusId { get; set; }
        public Status? Status { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();
        public int? EventId { get; set; } // Opcional
        public Event? Event { get; set; }
    }
}
