using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StockTrackingSystem.Entities;
using StockTrackingSystem.Models.Warehouse;
using StockTrackingSystem.Services;

namespace StockTrackingSystem.Controllers
{
    [Route("api/warehouses")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;
        private readonly IMapper _mapper;

        public WarehousesController(IWarehouseService warehouseService, IMapper mapper)
        {
            _warehouseService = warehouseService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var warehouses = await _warehouseService.GetAllWarehousesAsync();
            return Ok(warehouses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var warehouse = await _warehouseService.GetWarehouseByIdAsync(id);
            if (warehouse == null)
                return NotFound();

            return Ok(warehouse);
        }

        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] Warehouse warehouse)
        //{
        //    try
        //    {
        //        var created = await _warehouseService.CreateWarehouseAsync(warehouse);
        //        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return Conflict(new { message = ex.Message });
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> Create(CreateWarehouseDto dto)
        {
            var created = await _warehouseService.CreateWarehouseAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, [FromBody] Warehouse warehouse)
        //{
        //    if (id != warehouse.Id)
        //        return BadRequest("ID eşleşmiyor.");

        //    try
        //    {
        //        var updated = await _warehouseService.UpdateWarehouseAsync(warehouse);
        //        if (!updated)
        //            return NotFound();

        //        return NoContent();
        //    }
        //    catch (InvalidOperationException ex)
        //    {
        //        return Conflict(new { message = ex.Message });
        //    }
        //}

        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, UpdateWarehouseDto dto)
        {
            var updated = await _warehouseService.ReplaceWarehouseAsync(id, dto);
            if (updated is null) return NotFound();
            return NoContent(); 
        }

        
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, JsonPatchDocument<UpdateWarehouseDto> patchDoc)
        {
            var entity = await _warehouseService.GetWarehouseByIdAsync(id);
            if (entity is null) return NotFound();

            var dtoToPatch = _mapper.Map<UpdateWarehouseDto>(entity);

            patchDoc.ApplyTo(dtoToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryValidateModel(dtoToPatch))
                return BadRequest(ModelState);

            var updated = await _warehouseService.ApplyPatchedAsync(id, dtoToPatch);
            if (updated is null) return NotFound(); 

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _warehouseService.DeleteWarehouseAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
