using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.DbContexts;
using StockTrackingSystem.Entities;
using StockTrackingSystem.Models.StockCard;
using StockTrackingSystem.Repositories;

namespace StockTrackingSystem.Services
{
    public class StockCardService : IStockCardService
    {
        private readonly IStockCardRepository _stockCardRepository;
        private readonly IMapper _mapper;
        private readonly StockContext _context; // Item/Warehouse existence ve ekstra sorgular için

        public StockCardService(IStockCardRepository stockCardRepository, IMapper mapper, StockContext context)
        {
            _stockCardRepository = stockCardRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<StockCardViewDto>> GetAllStockCardsAsync()
        {
            var entities = await _stockCardRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<StockCardViewDto>>(entities);
        }

        public async Task<StockCardViewDto> GetStockCardByIdAsync(int id)
        {
            var entity = await _stockCardRepository.GetByIdAsync(id);
            if (entity is null)
                throw new KeyNotFoundException("StockCard bulunamadı.");

            return _mapper.Map<StockCardViewDto>(entity);
        }

        public async Task<StockCardViewDto> CreateStockCardAsync(CreateStockCardDto dto)
        {
            // Basit validasyon (DTO anotasyonları dışında da kontrol edelim)
            if (dto.Quantity < 0 || (dto.CriticalLevel.HasValue && dto.CriticalLevel.Value < 0))
                throw new ArgumentException("Quantity/CriticalLevel 0'dan küçük olamaz.");

            // (Opsiyonel) İlgili Item/Warehouse var mı?
            var itemExists = await _context.Items.AnyAsync(i => i.Id == dto.ItemId);
            var whExists = await _context.Warehouses.AnyAsync(w => w.Id == dto.WarehouseId);
            if (!itemExists || !whExists)
                throw new ArgumentException("ItemId veya WarehouseId geçersiz.");

            // Aynı kombinasyondan zaten var mı?
            var existing = await _stockCardRepository.GetByItemAndWarehouseAsync(dto.ItemId, dto.WarehouseId);
            if (existing is not null)
                throw new InvalidOperationException("Aynı Item + Warehouse için kayıt zaten mevcut.");

            var entity = _mapper.Map<StockCard>(dto);
            await _stockCardRepository.AddStockCardAsync(entity);
            await _stockCardRepository.SaveAsync();

            // Include'lar ViewDto için gerekli adları doldursun
            entity = await _stockCardRepository.GetByIdAsync(entity.Id) ?? entity;
            return _mapper.Map<StockCardViewDto>(entity);
        }

        public async Task<StockCardViewDto> UpdateStockCardAsync(int id, UpdateStockCardDto dto)
        {
            if (dto.Quantity < 0 || (dto.CriticalLevel.HasValue && dto.CriticalLevel.Value < 0))
                throw new ArgumentException("Quantity/CriticalLevel 0'dan küçük olamaz.");

            var entity = await _stockCardRepository.GetByIdAsync(id);
            if (entity is null)
                throw new KeyNotFoundException("Güncellenecek StockCard bulunamadı.");

            // Sadece güncellenebilir alanlar:
            entity.Quantity = dto.Quantity;
            entity.CriticalLevel = dto.CriticalLevel;

            _stockCardRepository.UpdateStockCard(entity);
            await _stockCardRepository.SaveAsync();

            // Tekrar oku (include'larla)
            entity = await _stockCardRepository.GetByIdAsync(entity.Id) ?? entity;
            return _mapper.Map<StockCardViewDto>(entity);
        }

        public async Task<UpdateStockCardDto?> GetForPatchAsync(int id)
        {
            var entity = await _stockCardRepository.GetByIdAsync(id);
            if (entity is null) return null;

            return new UpdateStockCardDto
            {
                Quantity = entity.Quantity,
                CriticalLevel = entity.CriticalLevel
            };
        }

        public async Task<StockCardViewDto> ApplyPatchAsync(int id, UpdateStockCardDto dto)
        {
            if (dto.Quantity < 0 || (dto.CriticalLevel.HasValue && dto.CriticalLevel.Value < 0))
                throw new ArgumentException("Quantity/CriticalLevel 0'dan küçük olamaz.");

            var entity = await _stockCardRepository.GetByIdAsync(id);
            if (entity is null)
                throw new KeyNotFoundException("Güncellenecek StockCard bulunamadı.");

            entity.Quantity = dto.Quantity;
            entity.CriticalLevel = dto.CriticalLevel;

            _stockCardRepository.UpdateStockCard(entity);
            await _stockCardRepository.SaveAsync();

            entity = await _stockCardRepository.GetByIdAsync(entity.Id) ?? entity;
            return _mapper.Map<StockCardViewDto>(entity);
        }



        public async Task DeleteStockCardAsync(int id)
        {
            var entity = await _stockCardRepository.GetByIdAsync(id);
            if (entity is null)
                throw new KeyNotFoundException("Silinecek StockCard bulunamadı.");

            _stockCardRepository.DeleteStockCard(entity);
            await _stockCardRepository.SaveAsync();
        }

        // -------- Ekstra uçlar --------

        // Kritik seviyenin altındakiler
        public async Task<IEnumerable<StockCardViewDto>> GetLowStockAsync()
        {
            var low = await _context.StockCards
                .Include(x => x.Item)
                .Include(x => x.Warehouse)
                .Where(x => x.CriticalLevel.HasValue && x.Quantity < x.CriticalLevel.Value)
                .ToListAsync();

            return _mapper.Map<IEnumerable<StockCardViewDto>>(low);
        }

        // Belirli deponun stokları
        public async Task<IEnumerable<StockCardViewDto>> GetByWarehouseAsync(int warehouseId)
        {
            var list = await _context.StockCards
                .Include(x => x.Item)
                .Include(x => x.Warehouse)
                .Where(x => x.WarehouseId == warehouseId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<StockCardViewDto>>(list);
        }
    }
}
