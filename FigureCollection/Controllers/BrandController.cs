using FigureCollection.Classes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FigureCollection.Controllers
{
    [Route("api/Brands")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly DataContext _context;
        public BrandController(DataContext context)
        {
            _context = context;
        }
        // GET: api/Brands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            return Ok(await _context.Brands.ToListAsync());
        }

        // GET: api/Brands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            var brand = await _context.Brands.FindAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            return brand;
        }

        // PUT: api/Brands/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutBrand(Brand request)
        {

            var checkBrand = await _context.Brands.FindAsync(request.id);
            if (checkBrand == null)
            {
                return BadRequest("Brand Not Found.");
            }

            checkBrand.name = request.name;
            await _context.SaveChangesAsync();

            return Ok(await _context.Brands.ToListAsync());
        }

        // POST: api/Brands
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {

            var checkBrand = _context.Brands.Any(m => m.name == brand.name);
            if (checkBrand == true)
            {
                return BadRequest("Brand Already Excists.");
            }

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBrand", new { id = brand.id }, brand);
        }

        // DELETE: api/Brands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BrandExists(int id)
        {
            return _context.Brands.Any(e => e.id == id);
        }

    }
}
