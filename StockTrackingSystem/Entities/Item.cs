using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Entities
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<ItemProperty>? ItemProperties { get; set; }
    }
}
