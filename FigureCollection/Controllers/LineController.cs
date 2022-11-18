using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FigureCollection.Data;
using FigureCollection.Classes;

namespace FigureCollection.Controllers
{
    [Route("api/Lines")]
    [ApiController]
    public class LineController : ControllerBase
    {
        private readonly DataContext _context;

        public LineController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Line
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Line>>> GetLines()
        {
            return await _context.Lines.ToListAsync();
        }

        // GET: api/Line/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Line>> GetLine(int id)
        {
            var line = await _context.Lines.FindAsync(id);

            if (line == null)
            {
                return NotFound();
            }

            return line;
        }

        // PUT: api/Line/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutLine(Line request)
        {
            var checkLine = await _context.Lines.FindAsync(request.id);
            if (checkLine == null)
            {
                return BadRequest("Brand Not Found.");
            }

            checkLine.name = request.name;
            await _context.SaveChangesAsync();

            return Ok(await _context.Editions.ToListAsync());
        }

        // POST: api/Line
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Line>> PostLine(Line line)
        {
            var checkLines = _context.Lines.Any(m => m.name == line.name);
            if (checkLines == true)
            {
                return BadRequest("Line Already Excists.");
            }

            _context.Lines.Add(line);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLine", new { id = line.id }, line);
        }

        // DELETE: api/Line/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLine(int id)
        {
            var line = await _context.Lines.FindAsync(id);
            if (line == null)
            {
                return NotFound();
            }

            _context.Lines.Remove(line);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LineExists(int id)
        {
            return _context.Lines.Any(e => e.id == id);
        }
    }
}
