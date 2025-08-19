using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.DbContexts;
using StockTrackingSystem.Entities;

namespace StockTrackingSystem.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly StockContext _context;

        public WarehouseRepository(StockContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Warehouse>> GetAllAsync()
        {
            return await _context.Warehouses.ToListAsync();
        }

        public async Task<Warehouse?> GetByIdAsync(int id)
        {
            return await _context.Warehouses.FindAsync(id);
        }

        public async Task AddWarehouseAsync(Warehouse warehouse)
        {
            await _context.Warehouses.AddAsync(warehouse);
        }

        public void DeleteWarehouse(Warehouse warehouse)
        {
            _context.Warehouses.Remove(warehouse);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> WarehouseNameExistsAsync(string name, int? excludingId = null)
        {
            return await _context.Warehouses
                .AnyAsync(w => w.Name.ToLower() == name.ToLower().Trim()
                               && (!excludingId.HasValue || w.Id != excludingId.Value));
        }

    }
}
