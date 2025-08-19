using AutoMapper;
using StockTrackingSystem.Entities;
using StockTrackingSystem.Models.Warehouse;
using StockTrackingSystem.Repositories;

namespace StockTrackingSystem.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IMapper _mapper;

        public WarehouseService(IWarehouseRepository warehouseRepository, IMapper mapper)
        {
            _warehouseRepository = warehouseRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WarehouseDto>> GetAllWarehousesAsync()
            => (await _warehouseRepository.GetAllAsync())
               .Select(e => _mapper.Map<WarehouseDto>(e));
        //{
        //    return await _warehouseRepository.GetAllAsync();

        //}

        public async Task<WarehouseDto?> GetWarehouseByIdAsync(int id)
        {
            var e = await _warehouseRepository.GetByIdAsync(id);
            return e is null ? null : _mapper.Map<WarehouseDto>(e);
            //return await _warehouseRepository.GetByIdAsync(id);
        }

        //public async Task<Warehouse> CreateWarehouseAsync(Warehouse warehouse)
        //{

        //    if (await _warehouseRepository.WarehouseNameExistsAsync(warehouse.Name))
        //    {
        //        throw new InvalidOperationException("Bu isimde bir depo zaten mevcut.");
        //    }

        //    await _warehouseRepository.AddWarehouseAsync(warehouse);
        //    await _warehouseRepository.SaveAsync();
        //    return warehouse;
        //}

        public async Task<WarehouseDto> CreateWarehouseAsync(CreateWarehouseDto dto)
        {
            //var warehouse = new Warehouse { Name = dto.Name, Location = dto.Location };
            var entity = _mapper.Map<Warehouse>(dto);
            await _warehouseRepository.AddWarehouseAsync(entity);
            await _warehouseRepository.SaveAsync();
            //return warehouse;
            return _mapper.Map<WarehouseDto>(entity);
        }


        //public async Task<bool> UpdateWarehouseAsync(Warehouse warehouse)
        //{
        //    if (await _warehouseRepository.WarehouseNameExistsAsync(warehouse.Name, warehouse.Id))
        //        throw new InvalidOperationException("Bu isimde başka bir depo zaten mevcut.");

        //    try
        //    {
        //        await _warehouseRepository.UpdateWarehouseAsync(warehouse);
        //        await _warehouseRepository.SaveAsync();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public async Task<WarehouseDto?> ReplaceWarehouseAsync(int id, UpdateWarehouseDto dto)
        {
            var entity = await _warehouseRepository.GetByIdAsync(id);
            if (entity is null) return null;

            _mapper.Map(dto, entity);
            await _warehouseRepository.SaveAsync();

            return _mapper.Map<WarehouseDto>(entity);
        }

        public async Task<WarehouseDto?> ApplyPatchedAsync(int id, UpdateWarehouseDto dto)
        {
            var entity = await _warehouseRepository.GetByIdAsync(id);
            if (entity is null) return null;

            _mapper.Map(dto, entity);
            await _warehouseRepository.SaveAsync();

            return _mapper.Map<WarehouseDto>(entity);
        }

        public async Task<bool> DeleteWarehouseAsync(int id)
        {
            var warehouse = await _warehouseRepository.GetByIdAsync(id);
            if (warehouse == null)
                return false;

            _warehouseRepository.DeleteWarehouse(warehouse);
            await _warehouseRepository.SaveAsync();
            return true;
        }

        public async Task<bool> WarehouseNameExistsAsync(string name, int? excludingId = null)
        {
            return await _warehouseRepository.WarehouseNameExistsAsync(name, excludingId);
        }

    }
}
