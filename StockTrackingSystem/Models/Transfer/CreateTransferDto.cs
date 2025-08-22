using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Models.Transfer
{
    public class CreateTransferDto
    {
        [Required]
        public int FromWarehouseId { get; set; }

        [Required]
        public int ToWarehouseId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Miktar 1'den küçük olamaz.")]
        public int Amount { get; set; }

        public string? Note { get; set; }
    }
}
