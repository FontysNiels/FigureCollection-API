using FigureCollection.Migrations;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FigureCollection.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;
        }
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("googleId/{googleId}")]
        public async Task<ActionResult<User>> GetUserByGoogle(string googleId)
        {

            var selectedUser = await _context.Users.FirstOrDefaultAsync(us => us.googleId == googleId);
            if (selectedUser == null)
            {
                return NotFound("L");
            }

            return selectedUser;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutUser(User request)
        {

            var checkUser = await _context.Users.FindAsync(request.id);
            if (checkUser == null)
            {
                return BadRequest("User Not Found.");
            }

            checkUser.username = request.username;
            checkUser.googleId = request.googleId;
            await _context.SaveChangesAsync();

            return Ok(await _context.Users.ToListAsync());
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {

            var checkUser = _context.Users.Any(m => m.username == user.username);
            if (checkUser == true)
            {
                return BadRequest("User Already Excists.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
