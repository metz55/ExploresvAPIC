using System.ComponentModel.DataAnnotations;

namespace ExploresvAPIC.Models
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(25)]
        public string Name { get; set; } = default!;
    }
}
