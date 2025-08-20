using StockTrackingSystem.Models.StockCard;

namespace StockTrackingSystem.Services
{
    public interface IStockCardService
    {
        Task<IEnumerable<StockCardViewDto>> GetAllStockCardsAsync();
        Task<StockCardViewDto> GetStockCardByIdAsync(int id);
        Task<StockCardViewDto> CreateStockCardAsync(CreateStockCardDto dto);
        Task<StockCardViewDto> UpdateStockCardAsync(int id, UpdateStockCardDto dto);

        Task<UpdateStockCardDto?> GetForPatchAsync(int id);
        Task<StockCardViewDto> ApplyPatchAsync(int id, UpdateStockCardDto dto);

        Task DeleteStockCardAsync(int id);


        Task<IEnumerable<StockCardViewDto>> GetLowStockAsync();
        Task<IEnumerable<StockCardViewDto>> GetByWarehouseAsync(int warehouseId);
    }
}
