using StockTrackingSystem.Entities;

namespace StockTrackingSystem.Repositories
{
    public interface IWarehouseRepository
    {
        Task<IEnumerable<Warehouse>> GetAllAsync();        
        Task<Warehouse?> GetByIdAsync(int id);             
        Task AddWarehouseAsync(Warehouse warehouse);       
        void DeleteWarehouse(Warehouse warehouse);         
        Task SaveAsync();                                  
        Task<bool> WarehouseNameExistsAsync(string name, int? excludingId = null);

    }
}
