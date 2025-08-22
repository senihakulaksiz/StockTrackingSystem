using Microsoft.AspNetCore.Mvc;
using StockTrackingSystem.Entities.Enums;
using StockTrackingSystem.Models.Request;
using StockTrackingSystem.Services;

namespace StockTrackingSystem.Controllers
{
    [Route("api/requests")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _service;

        public RequestsController(IRequestService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestViewDto>>> GetAll()
        {
            var list = await _service.GetAllRequestsAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}", Name = "GetRequestById")]
        public async Task<ActionResult<RequestViewDto>> GetById(int id)
        {
            var dto = await _service.GetRequestByIdAsync(id);
            if (dto is null) return NotFound("Request bulunamadı.");
            return Ok(dto);
        }

        [HttpGet("bywarehouse/{warehouseId:int}")]
        public async Task<ActionResult<IEnumerable<RequestViewDto>>> GetByWarehouse(int warehouseId)
        {
            var list = await _service.GetByWarehouseAsync(warehouseId);
            return Ok(list);
        }

        [HttpGet("byitem/{itemId:int}")]
        public async Task<ActionResult<IEnumerable<RequestViewDto>>> GetByItem(int itemId)
        {
            var list = await _service.GetByItemAsync(itemId);
            return Ok(list);
        }

        [HttpGet("bystatus/{status}")]
        public async Task<ActionResult<IEnumerable<RequestViewDto>>> GetByStatus(RequestStatus status)
        {
            var list = await _service.GetByStatusAsync(status);
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult<RequestViewDto>> Create([FromBody] CreateRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var created = await _service.CreateRequestAsync(dto);
                return CreatedAtRoute("GetRequestById", new { id = created.Id }, created);
            }
            catch (ArgumentException ex)            
            {
                return BadRequest(ex.Message);
            }
        }

        //Amount + RequestNote, sadece Pending iken
        [HttpPut("{id:int}")]
        public async Task<ActionResult<RequestViewDto>> Update(int id, [FromBody] UpdateRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updated = await _service.UpdateRequestAsync(id, dto);
                if (updated is null) return NotFound("Güncellenecek request bulunamadı.");
                return Ok(updated);
            }
            catch (ArgumentException ex)            
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)     
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPatch("{id:int}/status")]
        public async Task<ActionResult<RequestViewDto>> ChangeStatus(int id, [FromQuery] RequestStatus value)
        {
            try
            {
                var updated = await _service.ChangeStatusAsync(id, value);
                if (updated is null) return NotFound("Güncellenecek request bulunamadı.");
                return Ok(updated);
            }
            catch (ArgumentException ex)            
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)     
            {
                return Conflict(ex.Message);
            }
        }

        //Sadece Pending iken
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ok = await _service.DeleteRequestAsync(id);
                if (!ok) return NotFound("Silinecek request bulunamadı.");
                return NoContent();
            }
            catch (InvalidOperationException ex)   
            {
                return Conflict(ex.Message);
            }
        }
    }
}
