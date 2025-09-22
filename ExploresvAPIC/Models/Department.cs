using System.ComponentModel.DataAnnotations;

namespace ExploresvAPIC.Models
{
    public class Department
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Name { get; set; } = default!;
    }
}
