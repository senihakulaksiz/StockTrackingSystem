using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace StockTrackingSystem.Entities
{
    [Index(nameof(ItemId), nameof(WarehouseId), IsUnique = true)]
    public class StockCard
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        public int? CriticalLevel { get; set; }

        public Item Item { get; set; } = null!;
        public Warehouse Warehouse { get; set; } = null!;
    }
}
