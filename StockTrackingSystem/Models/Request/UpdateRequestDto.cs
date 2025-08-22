using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Models.Request
{
    public class UpdateRequestDto
    {
        [Range(1, int.MaxValue)]
        public int Amount { get; set; }

        [MaxLength(200)]
        public string? RequestNote { get; set; }
    }
}
