using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Models.Warehouse
{
    public class UpdateWarehouseDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        public bool IsMainWarehouse { get; set; }
    }
}