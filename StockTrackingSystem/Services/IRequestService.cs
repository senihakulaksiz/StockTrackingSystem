using StockTrackingSystem.Entities.Enums;
using StockTrackingSystem.Models.Request;

namespace StockTrackingSystem.Services
{
    public interface IRequestService
    {
        Task<IEnumerable<RequestViewDto>> GetAllRequestsAsync();
        Task<RequestViewDto?> GetRequestByIdAsync(int id);
        Task<RequestViewDto> CreateRequestAsync(CreateRequestDto dto);
        Task<RequestViewDto?> UpdateRequestAsync(int id, UpdateRequestDto dto);
        Task<RequestViewDto?> ChangeStatusAsync(int id, RequestStatus newStatus);
        Task<bool> DeleteRequestAsync(int id);
        Task<IEnumerable<RequestViewDto>> GetByWarehouseAsync(int warehouseId);
        Task<IEnumerable<RequestViewDto>> GetByItemAsync(int itemId);
        Task<IEnumerable<RequestViewDto>> GetByStatusAsync(RequestStatus status);
    }
}
