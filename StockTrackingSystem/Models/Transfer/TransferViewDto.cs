namespace StockTrackingSystem.Models.Transfer
{
    public class TransferViewDto
    {
        public int Id { get; set; }
        public int FromWarehouseId { get; set; }
        public string FromWarehouseName { get; set; } = string.Empty;
        public int ToWarehouseId { get; set; }
        public string ToWarehouseName { get; set; } = string.Empty;
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int Amount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string? Note { get; set; }
    }
}
