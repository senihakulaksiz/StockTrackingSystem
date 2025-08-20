using StockTrackingSystem.Entities;

namespace StockTrackingSystem.Repositories
{
    public interface IStockCardRepository
    {
        Task<IEnumerable<StockCard>> GetAllAsync();
        Task<StockCard?> GetByIdAsync(int id);
        Task<StockCard?> GetByItemAndWarehouseAsync(int itemId, int warehouseId);
        Task AddStockCardAsync(StockCard stockCard);
        void UpdateStockCard(StockCard stockCard);
        void DeleteStockCard(StockCard stockCard);

        Task SaveAsync();
    }
}