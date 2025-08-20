using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.DbContexts;
using StockTrackingSystem.Entities;

namespace StockTrackingSystem.Repositories
{
    public class StockCardRepository : IStockCardRepository
    {
        private readonly StockContext _context;

        public StockCardRepository(StockContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StockCard>> GetAllAsync()
        {
            return await _context.StockCards
                .Include(sc => sc.Item)
                .Include(sc => sc.Warehouse)
                .ToListAsync();
        }

        public async Task<StockCard?> GetByIdAsync(int id)
        {
            return await _context.StockCards
                .Include(sc => sc.Item)
                .Include(sc => sc.Warehouse)
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public async Task<StockCard?> GetByItemAndWarehouseAsync(int itemId, int warehouseId)
        {
            return await _context.StockCards
                .FirstOrDefaultAsync(sc => sc.ItemId == itemId && sc.WarehouseId == warehouseId);
        }

        public async Task AddStockCardAsync(StockCard stockCard)
        {
            await _context.StockCards.AddAsync(stockCard);
        }

        public void UpdateStockCard(StockCard stockCard)
        {
            _context.StockCards.Update(stockCard);
        }

        public void DeleteStockCard(StockCard stockCard)
        {
            _context.StockCards.Remove(stockCard);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
