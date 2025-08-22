using StockTrackingSystem.Entities;
using StockTrackingSystem.Entities.Enums;

namespace StockTrackingSystem.Repositories
{
    public interface IRequestRepository
    {
        Task<IEnumerable<Request>> GetAllAsync();
        Task<Request?> GetByIdAsync(int id);
        Task<IEnumerable<Request>> GetByWarehouseAsync(int warehouseId);
        Task<IEnumerable<Request>> GetByItemAsync(int itemId);
        Task<IEnumerable<Request>> GetByStatusAsync(RequestStatus status);
        Task AddRequestAsync(Request request);
        void UpdateRequest(Request request);
        void DeleteRequest(Request request);
        Task SaveAsync();
    }
}
