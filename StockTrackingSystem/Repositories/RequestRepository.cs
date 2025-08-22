using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.DbContexts;
using StockTrackingSystem.Entities;
using StockTrackingSystem.Entities.Enums;

namespace StockTrackingSystem.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly StockContext _context;

        public RequestRepository(StockContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Request>> GetAllAsync()
        {
            return await _context.Requests
                .Include(r => r.FromWarehouse)
                .Include(r => r.ToWarehouse)
                .Include(r => r.Item)
                .ToListAsync();
        }

        public async Task<Request?> GetByIdAsync(int id)
        {
            return await _context.Requests
                .Include(r => r.FromWarehouse)
                .Include(r => r.ToWarehouse)
                .Include(r => r.Item)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Request>> GetByWarehouseAsync(int warehouseId)
        {
            return await _context.Requests
                .Include(r => r.FromWarehouse)
                .Include(r => r.ToWarehouse)
                .Include(r => r.Item)
                .Where(r => r.FromWarehouseId == warehouseId || r.ToWarehouseId == warehouseId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Request>> GetByItemAsync(int itemId)
        {
            return await _context.Requests
                .Include(r => r.FromWarehouse)
                .Include(r => r.ToWarehouse)
                .Include(r => r.Item)
                .Where(r => r.ItemId == itemId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Request>> GetByStatusAsync(RequestStatus status)
        {
            return await _context.Requests
                .Include(r => r.FromWarehouse)
                .Include(r => r.ToWarehouse)
                .Include(r => r.Item)
                .Where(r => r.Status == status)
                .ToListAsync();
        }

        public async Task AddRequestAsync(Request request)
        {
            await _context.Requests.AddAsync(request);
        }

        public void UpdateRequest(Request request)
        {
            _context.Requests.Update(request);
        }

        public void DeleteRequest(Request request)
        {
            _context.Requests.Remove(request);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

