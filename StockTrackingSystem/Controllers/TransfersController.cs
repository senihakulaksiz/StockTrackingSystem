using Microsoft.AspNetCore.Mvc;
using StockTrackingSystem.Models.Transfer;
using StockTrackingSystem.Services;

namespace StockTrackingSystem.Controllers
{
    [Route("api/transfers")]
    [ApiController]
    public class TransfersController : ControllerBase
    {
        private readonly ITransferService _transferService;

        public TransfersController(ITransferService transferService)
        {
            _transferService = transferService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransferViewDto>>> GetAll()
        {
            var list = await _transferService.GetAllTransfersAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}", Name = "GetTransferById")]
        public async Task<ActionResult<TransferViewDto>> GetById(int id)
        {
            var dto = await _transferService.GetTransferByIdAsync(id);
            if (dto is null) return NotFound("Transfer bulunamadı.");
            return Ok(dto);
        }

        [HttpGet("bywarehouse/{warehouseId:int}")]
        public async Task<ActionResult<IEnumerable<TransferViewDto>>> GetByWarehouse(int warehouseId)
        {
            var list = await _transferService.GetByWarehouseAsync(warehouseId);
            return Ok(list);
        }

        [HttpGet("byitem/{itemId:int}")]
        public async Task<ActionResult<IEnumerable<TransferViewDto>>> GetByItem(int itemId)
        {
            var list = await _transferService.GetByItemAsync(itemId);
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult<TransferViewDto>> Create([FromBody] CreateTransferDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var created = await _transferService.CreateTransferAsync(dto);
                return CreatedAtRoute("GetTransferById", new { id = created.Id }, created);
            }
            catch (ArgumentException ex)            // geçersiz id/amount vs.
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)     // stok yetersiz vb.
            {
                return Conflict(ex.Message);
            }
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult<TransferViewDto>> Update(int id, [FromBody] UpdateTransferDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _transferService.UpdateTransferAsync(id, dto);
            if (updated is null) return NotFound("Güncellenecek transfer bulunamadı.");

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ok = await _transferService.DeleteTransferAsync(id);
                if (!ok) return NotFound("Silinecek transfer bulunamadı.");
                return NoContent();
            }
            catch (InvalidOperationException ex)     // geri alma için hedef stok yetersiz vb.
            {
                return Conflict(ex.Message);
            }
        }
    }
}
