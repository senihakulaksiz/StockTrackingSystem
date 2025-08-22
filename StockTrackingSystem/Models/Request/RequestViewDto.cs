using StockTrackingSystem.Entities.Enums;

namespace StockTrackingSystem.Models.Request
{
    public class RequestViewDto
    {
        public int Id { get; set; }
        public int FromWarehouseId { get; set; }
        public string FromWarehouseName { get; set; } = null!;
        public int ToWarehouseId { get; set; }
        public string ToWarehouseName { get; set; } = null!;
        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public int Amount { get; set; }
        public RequestStatus Status { get; set; }
        public DateTimeOffset RequestDate { get; set; }
        public DateTimeOffset? DecisionDate { get; set; }
        public string? RequestNote { get; set; }
    }
}
