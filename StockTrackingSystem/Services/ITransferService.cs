using StockTrackingSystem.Models.Transfer;

namespace StockTrackingSystem.Services
{
    public interface ITransferService
    {
        Task<IEnumerable<TransferViewDto>> GetAllTransfersAsync();
        Task<TransferViewDto?> GetTransferByIdAsync(int id);

        // Create: iş kuralları + transaction + stok güncelleme bu metotta yapılacak
        Task<TransferViewDto> CreateTransferAsync(CreateTransferDto dto);
        Task<TransferViewDto?> UpdateTransferAsync(int id, UpdateTransferDto dto);
        Task<bool> DeleteTransferAsync(int id);
        Task<IEnumerable<TransferViewDto>> GetByWarehouseAsync(int warehouseId);
        Task<IEnumerable<TransferViewDto>> GetByItemAsync(int itemId);
    }
}
