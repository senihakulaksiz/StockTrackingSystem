using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Models.Request
{
    public class CreateRequestDto
    {
        [Required] 
        public int FromWarehouseId { get; set; }

        [Required] 
        public int ToWarehouseId { get; set; }

        [Required] 
        public int ItemId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Amount 0’dan küçük olamaz.")]
        public int Amount { get; set; }

        [MaxLength(200)] 
        public string? RequestNote { get; set; }
    }
}
