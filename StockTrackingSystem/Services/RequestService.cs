using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.DbContexts;
using StockTrackingSystem.Entities;
using StockTrackingSystem.Entities.Enums;
using StockTrackingSystem.Models.Request;
using StockTrackingSystem.Repositories;

namespace StockTrackingSystem.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IMapper _mapper;
        private readonly StockContext _context;

        public RequestService(IRequestRepository requestRepository, IMapper mapper, StockContext context)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<RequestViewDto>> GetAllRequestsAsync()
        {
            var entities = await _requestRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RequestViewDto>>(entities);
        }

        public async Task<RequestViewDto?> GetRequestByIdAsync(int id)
        {
            var entity = await _requestRepository.GetByIdAsync(id);
            return entity is null ? null : _mapper.Map<RequestViewDto>(entity);
        }

        public async Task<IEnumerable<RequestViewDto>> GetByWarehouseAsync(int warehouseId)
        {
            var list = await _requestRepository.GetByWarehouseAsync(warehouseId);
            return _mapper.Map<IEnumerable<RequestViewDto>>(list);
        }

        public async Task<IEnumerable<RequestViewDto>> GetByItemAsync(int itemId)
        {
            var list = await _requestRepository.GetByItemAsync(itemId);
            return _mapper.Map<IEnumerable<RequestViewDto>>(list);
        }

        public async Task<IEnumerable<RequestViewDto>> GetByStatusAsync(RequestStatus status)
        {
            var list = await _requestRepository.GetByStatusAsync(status);
            return _mapper.Map<IEnumerable<RequestViewDto>>(list);
        }

        public async Task<RequestViewDto> CreateRequestAsync(CreateRequestDto dto)
        {
            if (dto.Amount <= 0)
                throw new ArgumentException("Amount 0'dan büyük olmalıdır.");

            if (dto.FromWarehouseId == dto.ToWarehouseId)
                throw new ArgumentException("FromWarehouseId ve ToWarehouseId aynı olamaz.");

            var itemExists = await _context.Items.AnyAsync(i => i.Id == dto.ItemId);
            var fromExists = await _context.Warehouses.AnyAsync(w => w.Id == dto.FromWarehouseId);
            var toExists = await _context.Warehouses.AnyAsync(w => w.Id == dto.ToWarehouseId);

            if (!itemExists || !fromExists || !toExists)
                throw new ArgumentException("ItemId veya WarehouseId geçersiz.");

            var entity = _mapper.Map<Request>(dto);

            await _requestRepository.AddRequestAsync(entity);
            await _requestRepository.SaveAsync();

            entity = await _requestRepository.GetByIdAsync(entity.Id) ?? entity;
            return _mapper.Map<RequestViewDto>(entity);
        }

        public async Task<RequestViewDto?> UpdateRequestAsync(int id, UpdateRequestDto dto)
        {
            if (dto.Amount <= 0)
                throw new ArgumentException("Amount 0’dan büyük olmalıdır.");

            var entity = await _requestRepository.GetByIdAsync(id);
            if (entity is null)
                return null;

            if (entity.Status != RequestStatus.Pending)
                throw new InvalidOperationException("Sadece Pending durumundaki talepler güncellenebilir.");

            entity.Amount = dto.Amount;
            entity.RequestNote = dto.RequestNote;

            _requestRepository.UpdateRequest(entity);
            await _requestRepository.SaveAsync();

            entity = await _requestRepository.GetByIdAsync(entity.Id) ?? entity;
            return _mapper.Map<RequestViewDto>(entity);
        }

        public async Task<RequestViewDto?> ChangeStatusAsync(int id, RequestStatus newStatus)
        {
            var entity = await _requestRepository.GetByIdAsync(id);
            if (entity is null)
                return null;

            if (entity.Status != RequestStatus.Pending)
                throw new InvalidOperationException("Yalnızca Pending durumundaki talepler güncellenebilir.");

            if (newStatus == RequestStatus.Pending)
                throw new ArgumentException("Yeni durum Pending olamaz.");

            if (newStatus != RequestStatus.Approved && newStatus != RequestStatus.Rejected)
                throw new ArgumentException("Geçerli durumlar: Approved veya Rejected.");

            entity.Status = newStatus;
            entity.DecisionDate = DateTimeOffset.UtcNow;

            _requestRepository.UpdateRequest(entity);
            await _requestRepository.SaveAsync();

            entity = await _requestRepository.GetByIdAsync(entity.Id) ?? entity;
            return _mapper.Map<RequestViewDto>(entity);
        }

        public async Task<bool> DeleteRequestAsync(int id)
        {
            var entity = await _requestRepository.GetByIdAsync(id);
            if (entity is null)
                return false;

            if (entity.Status != RequestStatus.Pending)
                throw new InvalidOperationException("Sadece Pending durumundaki talepler silinebilir.");

            _requestRepository.DeleteRequest(entity);
            await _requestRepository.SaveAsync();
            return true;
        }
    }
}
