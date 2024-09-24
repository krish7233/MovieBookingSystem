using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.AppDBContexts;
using MovieBookingSystem.Models;

namespace MovieBookingSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CastsController : ControllerBase
    {
        private readonly MovieBookingDBContext _context;

        public CastsController(MovieBookingDBContext context)
        {
            _context = context;
        }

        // GET: api/Casts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cast>>> GetCasts()
        {
            return await _context.Casts.ToListAsync();
        }

        // GET: api/Casts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cast>> GetCast(int id)
        {
            var cast = await _context.Casts.FindAsync(id);

            if (cast == null)
            {
                return NotFound();
            }

            return cast;
        }

        // PUT: api/Casts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCast(int id, Cast cast)
        {
            if (id != cast.Id)
            {
                return BadRequest();
            }

            _context.Entry(cast).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CastExists(id))
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

        // POST: api/Casts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cast>> PostCast(Cast cast)
        {
            _context.Casts.Add(cast);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCast", new { id = cast.Id }, cast);
        }

        // DELETE: api/Casts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCast(int id)
        {
            var cast = await _context.Casts.FindAsync(id);
            if (cast == null)
            {
                return NotFound();
            }

            _context.Casts.Remove(cast);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CastExists(int id)
        {
            return _context.Casts.Any(e => e.Id == id);
        }
    }
}
