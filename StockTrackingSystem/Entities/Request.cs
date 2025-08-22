using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace StockTrackingSystem.Entities
{
    [Index(nameof(Status))]
    public class Request
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

        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public DateTimeOffset RequestDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? DecisionDate { get; set; }

        [MaxLength(200)]
        public string? RequestNote { get; set; }

        public Warehouse FromWarehouse { get; set; } = null!;
        public Warehouse ToWarehouse { get; set; } = null!;
        public Item Item { get; set; } = null!;
    }
}
