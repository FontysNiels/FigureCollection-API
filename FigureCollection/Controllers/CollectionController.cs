using FigureCollection.Classes;
using FigureCollection.Migrations;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace FigureCollection.Controllers
{
    [Route("api/Collection")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly DataContext _context;
        public CollectionController(DataContext context)
        {
            _context = context;
        }
        // GET: api/Collection
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Collection>>> GetCollections()
        {
            return Ok(await _context.Collections
                .Include(x => x.user)
                .Include(x => x.figure)
                    .Include(x => x.figure.Brand)
                    .Include(x => x.figure.Manufacturer)
                    .Include(x => x.figure.Character)
                    .Include(x => x.figure.Line)
                    .Include(x => x.figure.Edition).ToListAsync());
        }

        //Get all collected from 1 user
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Collection>>> GetCollectionByUser(int userId)
        {

            var selectedCollection = await _context.Collections
                .Include(x => x.user)
                .Include(x => x.figure)
                    .Include(x => x.figure.Brand)
                    .Include(x => x.figure.Manufacturer)
                    .Include(x => x.figure.Character)
                    .Include(x => x.figure.Line)
                    .Include(x => x.figure.Edition)
                .Where(us => us.user.id == userId).ToListAsync();

            if (selectedCollection.Count == 0)
            {
                return NotFound("L");
            }

            return selectedCollection;
        }

        [HttpGet("item")]
        public async Task<ActionResult<Collection>> CheckIfCollected(string username, int figureId)
        {

            var selectedCollection = await _context.Collections
                .Include(x => x.user)
                .Include(x => x.figure)
                    .Include(x => x.figure.Brand)
                    .Include(x => x.figure.Manufacturer)
                    .Include(x => x.figure.Character)
                    .Include(x => x.figure.Line)
                    .Include(x => x.figure.Edition)
                .FirstOrDefaultAsync(us => us.user.username == username && us.figure.id == figureId );
            if (selectedCollection == null)
            {
                return NotFound("Not found");
            }

            return selectedCollection;
        }

        // PUT: api/Collection/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutCollection(Collection request)
        {

            var checkCollection = await _context.Collections.FindAsync(request.id);
            if (checkCollection == null)
            {
                return BadRequest("Collection Not Found.");
            }

            checkCollection.user.id = request.user.id;
            checkCollection.figure.id = request.figure.id;
            await _context.SaveChangesAsync();

            return Ok(await _context.Collections.ToListAsync());
        }

        // POST: api/Collection
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Collection>> PostCollection(string user, int figure)
        {

            //var checkCollection = _context.Collections.Any(m => m.userid.id == collection.userid.id &&
            //                                            m.figureid.id == collection.figureid.id);

            var checkUser = await _context.Users.FirstOrDefaultAsync(us => us.username == user);
            var checkFigure = await _context.Figures.FirstOrDefaultAsync(us => us.id == figure);

            var checkCollection = _context.Collections.Any(m => m.user.username == user &&
                                            m.figure.id == figure);
            if (checkUser == null)
            {
                return BadRequest("User does not Excists.");
            }
            if (checkFigure == null)
            {
                return BadRequest("Figure does not Excists.");
            }
            if (checkCollection == true)
            {
                return BadRequest("Collection Already Excists.");
            }


            Collection newItem = new Collection()
            {
                user = checkUser,
                figure = checkFigure
            };

            _context.Collections.Add(newItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCollections", new { id = newItem.id }, newItem);
        }

        // DELETE: api/Collection/5
        [HttpDelete]
        public async Task<IActionResult> DeleteCollection(string user, int figure)
        {
            var checkCollection = _context.Collections.Any(m => m.user.username == user &&
                                                                            m.figure.id == figure);
            if (checkCollection == false)
            {
                return NotFound("");
            }

            Collection removedItem = _context.Collections.FirstOrDefault(x => x.user.username == user && x.figure.id == figure);
            _context.Collections.Remove(removedItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
