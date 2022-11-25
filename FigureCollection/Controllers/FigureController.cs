using FigureCollection.Classes;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FigureCollection.Controllers
{
    [Route("api/Figures")]
    [ApiController]
    public class FigureController : ControllerBase
    {
        private readonly DataContext _context;
        public FigureController(DataContext context)
        {
            _context = context;
        }

        //GET ALL
        [HttpGet]
        public async Task<ActionResult<List<Figure>>> Get()
        {
            return Ok(await _context.Figures.Include(x => x.Brand)
                                            .Include(x => x.Manufacturer)
                                            .Include(x => x.Character)
                                            .Include(x => x.Line)
                                            .Include(x => x.Edition).ToListAsync());
        }
        //GET ON ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Figure>> Get(int id)
        {
            var figure =  _context.Figures.Where(f => f.id == id)
                                            .Include(x => x.Brand)
                                            .Include(x => x.Manufacturer)
                                            .Include(x => x.Character)
                                            .Include(x => x.Line)
                                            .Include(x => x.Edition);
            if (figure == null)
            {
                return BadRequest("Hero Not Found.");
            }
            return Ok(figure);
        }

        //GET ON BRAND ID
        [HttpGet("brand/{BrandId}")]
        public async Task<ActionResult<Figure>> GetFromBrand(int BrandId)
        {
            var figure = _context.Figures.Where(f => f.Brand.id == BrandId)
                                            .Include(x => x.Brand)
                                            .Include(x => x.Manufacturer)
                                            .Include(x => x.Character)
                                            .Include(x => x.Line)
                                            .Include(x => x.Edition);
            if (figure == null)
            {
                return BadRequest("Hero Not Found.");
            }
            return Ok(figure);
        }
        //GET ON BRAND ID
        [HttpGet("manufacturer/{ManufacturerId}")]
        public async Task<ActionResult<Figure>> GetFromManufacturerId(int ManufacturerId)
        {
            var figure = _context.Figures.Where(f => f.Manufacturer.id == ManufacturerId)
                                            .Include(x => x.Brand)
                                            .Include(x => x.Manufacturer)
                                            .Include(x => x.Character)
                                            .Include(x => x.Line)
                                            .Include(x => x.Edition);
            if (figure == null)
            {
                return BadRequest("Hero Not Found.");
            }
            return Ok(figure);
        }
        //ADD NEW FIGURE (PURE FOR ADDING ANYTHING NEW)
        [HttpPost]
        public async Task<ActionResult<List<Figure>>> AddFigure(Figure figure)
        {
            Brand newBrand = figure.Brand;
            Manufacturer newManufacturer = figure.Manufacturer;
            Character newCharacter = figure.Character;
            Line newLine = figure.Line;
            Edition newEdion = figure.Edition;

            var checkbrand = await _context.Brands.FindAsync(newBrand.id);
            var checkManufacturer = await _context.Manufacturers.FindAsync(newManufacturer.id);
            var checkCharacter = await _context.Characters.FindAsync(newCharacter.id);
            var checkLine = await _context.Lines.FindAsync(newLine.id);
            var checkEdition = await _context.Editions.FindAsync(newEdion.id);

            //Checks if thing already excists, if not it just makes the new one
            if (checkbrand != null)
            {
                figure.Brand = checkbrand;
            }
            if (checkManufacturer != null)
            {
                figure.Manufacturer = checkManufacturer;
            }
            if (checkCharacter != null)
            {
                figure.Character = checkCharacter;
            }
            if (checkLine != null)
            {
                figure.Line = checkLine;
            }
            if (checkEdition != null)
            {
                figure.Edition = checkEdition;
            }

            _context.Figures.Add(figure);
            await _context.SaveChangesAsync();

            return Ok(await _context.Figures.ToListAsync());
        }
        //UPDATE FIGURE (Purely for updating the figures data, not the data from other classes)
        [HttpPut]
        public async Task<ActionResult<List<Figure>>> UpdateFigure(Figure request)
        {

            var figure = await _context.Figures.FindAsync(request.id);
            if (figure == null)
            {
                return BadRequest("Figure Not Found.");
            }
            var brand = await _context.Brands.FindAsync(request.Brand.id);
            if (brand == null)
            {
                return BadRequest("Brand Not Found.");
            }
            var manufacturer = await _context.Manufacturers.FindAsync(request.Manufacturer.id);
            if (brand == null)
            {
                return BadRequest("Manufacturer Not Found.");
            }
            var character = await _context.Characters.FindAsync(request.Character.id);
            if (character == null)
            {
                return BadRequest("Character Not Found.");
            }
            var line = await _context.Lines.FindAsync(request.Line.id);
            if (line == null)
            {
                return BadRequest("Line Not Found.");
            }
            var edition = await _context.Editions.FindAsync(request.Edition.id);
            if (edition == null)
            {
                return BadRequest("Edition Not Found.");
            }

            figure.name = request.name;
            figure.size = request.size;
            figure.scale = request.scale;

            figure.Brand = brand;
            figure.Manufacturer = manufacturer;
            figure.Character = character;
            figure.Line = line;
            figure.Edition = edition;

            await _context.SaveChangesAsync();

            return Ok(await _context.Figures.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Figure>>> Delete(int id)
        {
            var figure = await _context.Figures.FindAsync(id);
            if (figure == null)
            {
                return BadRequest("Figure Not Found.");
            }

            _context.Figures.Remove(figure);
            await _context.SaveChangesAsync();

            return Ok(await _context.Figures.ToListAsync());
        }

    }
}
