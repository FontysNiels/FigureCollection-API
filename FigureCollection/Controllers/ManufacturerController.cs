using FigureCollection.Classes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace FigureCollection.Controllers
{
    [Route("api/Manufacturers")]
    [ApiController]
    public class ManufacturerController : ControllerBase
    {
        private readonly DataContext _context;
        public ManufacturerController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Manufacturer>>> Get()
        {
            return Ok(await _context.Manufacturers.ToListAsync());
        }
        //CREATE NEW
        [HttpPost]
        public async Task<ActionResult<List<Manufacturer>>> AddManufacturer(Manufacturer newMan)
        {
            var checkManufacturer = _context.Manufacturers.Any(m => m.name == newMan.name);
            if (checkManufacturer == true)
            {
                return BadRequest("Manufacturer Already Excists.");
            }

            _context.Manufacturers.Add(newMan);
            await _context.SaveChangesAsync();

            return Ok(await _context.Manufacturers.ToListAsync());
        }
        //UPDATE
        [HttpPut]
        public async Task<ActionResult<List<Manufacturer>>> UpdateManufacturer(Manufacturer request)
        {

            var checkManufacturer = await _context.Manufacturers.FindAsync(request.id);
            if (checkManufacturer == null)
            {
                return BadRequest("Manufacturer Not Found.");
            }
            
            checkManufacturer.name = request.name;
            await _context.SaveChangesAsync();

            return Ok(await _context.Manufacturers.ToListAsync());
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Manufacturer>>> Delete(int id)
        {
            var manufacturer = await _context.Manufacturers.FindAsync(id);
            if (manufacturer == null)
            {
                return BadRequest("Manufacturer Not Found.");
            }

            _context.Manufacturers.Remove(manufacturer);
            await _context.SaveChangesAsync();

            return Ok(await _context.Manufacturers.ToListAsync());
        }
    }
}
