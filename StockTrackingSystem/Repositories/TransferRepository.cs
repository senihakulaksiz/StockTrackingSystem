using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.DbContexts;
using StockTrackingSystem.Entities;

namespace StockTrackingSystem.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        private readonly StockContext _context;

        public TransferRepository(StockContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transfer>> GetAllAsync()
        {
            return await _context.Transfers
                .Include(t => t.FromWarehouse)
                .Include(t => t.ToWarehouse)
                .Include(t => t.Item)
                .ToListAsync();
        }

        public async Task<Transfer?> GetByIdAsync(int id)
        {
            return await _context.Transfers
                .Include(t => t.FromWarehouse)
                .Include(t => t.ToWarehouse)
                .Include(t => t.Item)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Transfer>> GetByWarehouseAsync(int warehouseId)
        {
            return await _context.Transfers
                .Include(t => t.FromWarehouse)
                .Include(t => t.ToWarehouse)
                .Include(t => t.Item)
                .Where(t => t.FromWarehouseId == warehouseId || t.ToWarehouseId == warehouseId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transfer>> GetByItemAsync(int itemId)
        {
            return await _context.Transfers
                .Include(t => t.FromWarehouse)
                .Include(t => t.ToWarehouse)
                .Include(t => t.Item)
                .Where(t => t.ItemId == itemId)
                .ToListAsync();
        }

        public async Task AddTransferAsync(Transfer transfer)
        {
            await _context.Transfers.AddAsync(transfer);
        }

        public void UpdateTransfer(Transfer transfer)
        {
            _context.Transfers.Update(transfer);
        }

        public void DeleteTransfer(Transfer transfer)
        {
            _context.Transfers.Remove(transfer);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
