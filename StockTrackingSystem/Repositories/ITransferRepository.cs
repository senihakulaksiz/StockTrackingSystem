using StockTrackingSystem.Entities;

namespace StockTrackingSystem.Repositories
{
    public interface ITransferRepository
    {
        Task<IEnumerable<Transfer>> GetAllAsync();
        Task<Transfer?> GetByIdAsync(int id);
        Task<IEnumerable<Transfer>> GetByWarehouseAsync(int warehouseId);
        Task<IEnumerable<Transfer>> GetByItemAsync(int itemId);
        Task AddTransferAsync(Transfer transfer);
        void UpdateTransfer(Transfer transfer);
        void DeleteTransfer(Transfer transfer);
        Task SaveAsync();
    }
}