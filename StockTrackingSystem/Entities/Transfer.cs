using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Entities
{
    public class Transfer
    {
        [Key]
        public int Id { get; set; }

        [Required] 
        public int FromWarehouseId { get; set; }

        [Required] 
        public int ToWarehouseId { get; set; }

        [Required] 
        public int ItemId { get; set; }

        [Range(1, int.MaxValue)]
        public int Amount { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        [MaxLength(200)] 
        public string? Note { get; set; }

        public Warehouse? FromWarehouse { get; set; }
        public Warehouse? ToWarehouse { get; set; }
        public Item? Item { get; set; }
    }
}
