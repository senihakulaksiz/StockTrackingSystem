using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StockTrackingSystem.Models.StockCard;
using StockTrackingSystem.Services;

namespace StockTrackingSystem.Controllers
{
    [Route("api/stockcards")]
    [ApiController]
    public class StockCardsController : ControllerBase
    {
        private readonly IStockCardService _service;

        public StockCardsController(IStockCardService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StockCardViewDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllStockCardsAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(StockCardViewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetStockCardByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(StockCardViewDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateStockCardDto dto)
        {
            try
            {
                var created = await _service.CreateStockCardAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Aynı (ItemId, WarehouseId) kombosu varsa
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(StockCardViewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStockCardDto dto)
        {
            try
            {
                var updated = await _service.UpdateStockCardAsync(id, dto);
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(typeof(StockCardViewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<UpdateStockCardDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest(new { message = "Patch dokümanı boş olamaz." });

            var dtoToPatch = await _service.GetForPatchAsync(id);
            if (dtoToPatch is null)
                return NotFound(new { message = "StockCard bulunamadı." });

            patchDoc.ApplyTo(dtoToPatch, ModelState);

            if (!TryValidateModel(dtoToPatch))
                return ValidationProblem(ModelState);

            try
            {
                var updated = await _service.ApplyPatchAsync(id, dtoToPatch);
                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteStockCardAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("lowstock")]
        [ProducesResponseType(typeof(IEnumerable<StockCardViewDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLowStock()
        {
            var result = await _service.GetLowStockAsync();
            return Ok(result);
        }

        [HttpGet("bywarehouse/{warehouseId:int}")]
        [ProducesResponseType(typeof(IEnumerable<StockCardViewDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByWarehouse(int warehouseId)
        {
            var result = await _service.GetByWarehouseAsync(warehouseId);
            return Ok(result);
        }
    }
}
