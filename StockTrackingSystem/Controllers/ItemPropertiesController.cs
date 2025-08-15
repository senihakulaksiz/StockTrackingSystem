using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockTrackingSystem.DbContexts;
using StockTrackingSystem.Entities;

namespace StockTrackingSystem.Controllers
{
    [Route("api/itemproperties")]
    [ApiController]
    public class ItemPropertiesController : ControllerBase
    {
        private readonly StockContext _context;

        public ItemPropertiesController(StockContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemProperty>>> GetItemProperties()
        {
            return await _context.ItemProperties.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemProperty>> GetItemProperty(int id)
        {
            var itemProperty = await _context.ItemProperties.FindAsync(id);

            if (itemProperty == null)
            {
                return NotFound();
            }

            return itemProperty;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemProperty(int id, ItemProperty itemProperty)
        {
            if (id != itemProperty.Id)
            {
                return BadRequest();
            }

            _context.Entry(itemProperty).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemPropertyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ItemProperty>> PostItemProperty(ItemProperty itemProperty)
        {
            _context.ItemProperties.Add(itemProperty);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemProperty", new { id = itemProperty.Id }, itemProperty);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemProperty(int id)
        {
            var itemProperty = await _context.ItemProperties.FindAsync(id);
            if (itemProperty == null)
            {
                return NotFound();
            }

            _context.ItemProperties.Remove(itemProperty);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemPropertyExists(int id)
        {
            return _context.ItemProperties.Any(e => e.Id == id);
        }
    }
}
