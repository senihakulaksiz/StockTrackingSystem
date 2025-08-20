using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Models.StockCard
{
    public class UpdateStockCardDto
    {
        [Range(0, int.MaxValue, ErrorMessage = "Quantity 0'dan küçük olamaz.")]
        public int Quantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "CriticalLevel 0'dan küçük olamaz.")]
        public int? CriticalLevel { get; set; }
    }
}
