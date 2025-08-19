using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Models.Warehouse
{
    public class CreateWarehouseDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [Required]
        public string Location { get; set; } = null!;
        public bool IsMainWarehouse { get; set; } = false;
    }
}
