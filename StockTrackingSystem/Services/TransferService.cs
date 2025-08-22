using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.DbContexts;
using StockTrackingSystem.Entities;
using StockTrackingSystem.Models.Transfer;
using StockTrackingSystem.Repositories;

namespace StockTrackingSystem.Services
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IStockCardRepository _stockCardRepository;
        private readonly IMapper _mapper;
        private readonly StockContext _context;

        public TransferService(
            ITransferRepository transferRepository,
            IStockCardRepository stockCardRepository,
            IMapper mapper,
            StockContext context)
        {
            _transferRepository = transferRepository;
            _stockCardRepository = stockCardRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<TransferViewDto>> GetAllTransfersAsync()
        {
            var entities = await _transferRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TransferViewDto>>(entities);
        }

        public async Task<TransferViewDto?> GetTransferByIdAsync(int id)
        {
            var entity = await _transferRepository.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<TransferViewDto>(entity);
        }

        public async Task<TransferViewDto> CreateTransferAsync(CreateTransferDto dto)
        {
            if (dto.FromWarehouseId == dto.ToWarehouseId)
                throw new ArgumentException("FromWarehouseId ve ToWarehouseId aynı olamaz.");

            if (dto.Amount <= 0)
                throw new ArgumentException("Amount 0'dan küçük olamaz.");

            var itemExists = await _context.Items.AnyAsync(i => i.Id == dto.ItemId);
            if (!itemExists)
                throw new ArgumentException("Geçersiz ItemId.");

            var fromExists = await _context.Warehouses.AnyAsync(w => w.Id == dto.FromWarehouseId);
            var toExists = await _context.Warehouses.AnyAsync(w => w.Id == dto.ToWarehouseId);
            if (!fromExists || !toExists)
                throw new ArgumentException("Geçersiz FromWarehouseId veya ToWarehouseId.");

            // Transaction: stok düş/ekle + transfer kaydı tek seferde ----
            await using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                var fromSc = await _stockCardRepository
                    .GetByItemAndWarehouseAsync(dto.ItemId, dto.FromWarehouseId);

                if (fromSc is null)
                    throw new InvalidOperationException("Kaynak depoda bu ürün için stok kartı yok.");

                if (fromSc.Quantity < dto.Amount)
                    throw new InvalidOperationException("Kaynak depoda yeterli stok yok.");

                var toSc = await _stockCardRepository
                    .GetByItemAndWarehouseAsync(dto.ItemId, dto.ToWarehouseId);

                bool createdToSc = false;
                if (toSc is null)
                {
                    toSc = new StockCard
                    {
                        ItemId = dto.ItemId,
                        WarehouseId = dto.ToWarehouseId,
                        Quantity = 0
                    };
                    await _stockCardRepository.AddStockCardAsync(toSc);
                    createdToSc = true;
                }

                // Stok hareketleri
                fromSc.Quantity -= dto.Amount;
                toSc.Quantity += dto.Amount;

                _stockCardRepository.UpdateStockCard(fromSc);
                //_stockCardRepository.UpdateStockCard(toSc);
                if (!createdToSc)
                    _stockCardRepository.UpdateStockCard(toSc);

                // Transfer kaydı
                var transfer = _mapper.Map<Transfer>(dto);
                transfer.CreatedAt = DateTimeOffset.UtcNow;

                await _transferRepository.AddTransferAsync(transfer);

                await _transferRepository.SaveAsync();

                await tx.CommitAsync();

                // Navigation'lar dolu bir şekilde tekrar oku ve map et
                var created = await _transferRepository.GetByIdAsync(transfer.Id);
                return _mapper.Map<TransferViewDto>(created!);
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<TransferViewDto?> UpdateTransferAsync(int id, UpdateTransferDto dto)
        {
            var entity = await _transferRepository.GetByIdAsync(id);
            if (entity is null)
                return null;

            entity.Note = dto.Note;

            _transferRepository.UpdateTransfer(entity);
            await _transferRepository.SaveAsync();

            entity = await _transferRepository.GetByIdAsync(entity.Id) ?? entity;
            return _mapper.Map<TransferViewDto>(entity);
        }

        public async Task<bool> DeleteTransferAsync(int id)
        {
            // Gerçek dünyada transferler silinmez (tarihçe). Burada temel düzeyde:
            // - Silmeden önce stokları GERİ AL (reverse)
            var entity = await _transferRepository.GetByIdAsync(id);
            if (entity is null)
                return false;

            await using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                // Hedef stoktan düş, kaynak stoğa ekle
                var toSc = await _stockCardRepository
                    .GetByItemAndWarehouseAsync(entity.ItemId, entity.ToWarehouseId);
                if (toSc is null || toSc.Quantity < entity.Amount)
                    throw new InvalidOperationException("Silme için hedef depoda yeterli stok yok.");

                var fromSc = await _stockCardRepository
                    .GetByItemAndWarehouseAsync(entity.ItemId, entity.FromWarehouseId);

                if (fromSc is null)
                {
                    fromSc = new StockCard
                    {
                        ItemId = entity.ItemId,
                        WarehouseId = entity.FromWarehouseId,
                        Quantity = 0
                    };
                    await _stockCardRepository.AddStockCardAsync(fromSc);
                }

                toSc.Quantity -= entity.Amount;
                fromSc.Quantity += entity.Amount;

                _stockCardRepository.UpdateStockCard(toSc);
                _stockCardRepository.UpdateStockCard(fromSc);

                _transferRepository.DeleteTransfer(entity);

                await _transferRepository.SaveAsync();
                await tx.CommitAsync();
                return true;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<TransferViewDto>> GetByWarehouseAsync(int warehouseId)
        {
            var list = await _transferRepository.GetByWarehouseAsync(warehouseId);
            return _mapper.Map<IEnumerable<TransferViewDto>>(list);
        }

        public async Task<IEnumerable<TransferViewDto>> GetByItemAsync(int itemId)
        {
            var list = await _transferRepository.GetByItemAsync(itemId);
            return _mapper.Map<IEnumerable<TransferViewDto>>(list);
        }
    }
}
