using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Location { get; set; } = null!;

        public bool IsMainWarehouse { get; set; }
    }
}
