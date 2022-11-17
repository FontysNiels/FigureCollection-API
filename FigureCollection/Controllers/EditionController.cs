using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FigureCollection;
using FigureCollection.Data;

namespace FigureCollection.Controllers
{
    [Route("api/Editions")]
    [ApiController]
    public class EditionController : ControllerBase
    {
        private readonly DataContext _context;

        public EditionController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Edition
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Edition>>> GetEditions()
        {
            return await _context.Editions.ToListAsync();
        }

        // GET: api/Edition/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Edition>> GetEdition(int id)
        {
            var edition = await _context.Editions.FindAsync(id);

            if (edition == null)
            {
                return NotFound();
            }

            return edition;
        }

        // PUT: api/Edition/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutEdition(Edition request)
        {
            var checkEdition = await _context.Editions.FindAsync(request.id);
            if (checkEdition == null)
            {
                return BadRequest("Edition Not Found.");
            }

            checkEdition.name = request.name;
            await _context.SaveChangesAsync();

            return Ok(await _context.Editions.ToListAsync());
        }

        // POST: api/Edition
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Edition>> PostEdition(Edition edition)
        {
            var checkEditions = _context.Editions.Any(m => m.name == edition.name);
            if (checkEditions == true)
            {
                return BadRequest("Edition Already Excists.");
            }

            _context.Editions.Add(edition);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEdition", new { id = edition.id }, edition);
        }

        // DELETE: api/Edition/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEdition(int id)
        {
            var edition = await _context.Editions.FindAsync(id);
            if (edition == null)
            {
                return NotFound();
            }

            _context.Editions.Remove(edition);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EditionExists(int id)
        {
            return _context.Editions.Any(e => e.id == id);
        }
    }
}
