namespace StockTrackingSystem.Models.StockCard
{
    public class StockCardViewDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;

        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;

        public int Quantity { get; set; }
        public int? CriticalLevel { get; set; }
    }
}
