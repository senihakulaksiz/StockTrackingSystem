using StockTrackingSystem.Models.Warehouse;

namespace StockTrackingSystem.Services
{
    public interface IWarehouseService
    {
        Task<IEnumerable<WarehouseDto>> GetAllWarehousesAsync();
        Task<WarehouseDto?> GetWarehouseByIdAsync(int id);
        //Task<Warehouse> CreateWarehouseAsync(Warehouse warehouse);
        Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseDto dto);
        //Task<bool> UpdateWarehouseAsync(Warehouse warehouse);
        Task<WarehouseDto?> ReplaceWarehouseAsync(int id, UpdateWarehouseDto dto);
        Task<WarehouseDto?> ApplyPatchedAsync(int id, UpdateWarehouseDto dto);  
        Task<bool> DeleteWarehouseAsync(int id);
        Task<bool> WarehouseNameExistsAsync(string name, int? excludingId = null);
    }
}