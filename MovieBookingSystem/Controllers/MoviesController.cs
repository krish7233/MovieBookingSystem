using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.AppDBContexts;
using MovieBookingSystem.DTOs;
using MovieBookingSystem.Models;

namespace MovieBookingSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieBookingDBContext _context;

        public MoviesController(MovieBookingDBContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [Authorize(Roles = "user,admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> Getmovies()
        {
            var result = await _context.movies.Include(m=>m.Bookings).ToListAsync();
            var movies = GetMoviesDTO(result);
                        
            return movies;
        }

        // GET: api/Movies/5
        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDTO>> GetMovie(int id)
        {
            if(id < 0)
            {
                throw new InvalidDataException("Id is not valid. please enter correct valid id.");
            }
            
            var movie = await _context.movies.Include(m => m.Bookings).Include(m => m.Casts).FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var bookingLst = new List<BookingDTO>();

            foreach(var booking in movie.Bookings)
            {
                bookingLst.Add(new BookingDTO { Id = booking.Id, BookingTime = booking.BookingTime, UserId = booking.UserId});
            }

            var castLst = new List<CastDTO>();

            foreach(var cast in movie.Casts)
            {
                castLst.Add(new CastDTO { Id = cast.Id, Description = cast.Description, Name = cast.Name });
            }

            var movieDTO = new MovieDTO { Id = movie.Id, Title = movie.Title, Description = movie.Description, ReleaseDate = movie.ReleaseDate, Bookings = bookingLst, casts = castLst};

            return movieDTO;
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(MovieDTO movieDTO)
        {
            var movieCasts = new List<Cast>();
            var movieBookings = new List<Booking>();

            foreach(var cast in movieDTO.casts)
            {
                movieCasts.Add(new Cast { Id = cast.Id, Name = cast.Name, Description = cast.Description });
            }

            foreach(var booking in movieDTO.Bookings)
            {
                movieBookings.Add(new Booking { Id = booking.Id, UserId =  booking.UserId, BookingTime = booking.BookingTime });
            }

            var movie = new Movie() { Title = movieDTO.Title, Description = movieDTO.Description, ReleaseDate = movieDTO.ReleaseDate, Casts = movieCasts, Bookings = movieBookings};

            _context.movies.Add(movie);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// api/movies/search
        /// </summary>
        /// <param name="releaseDateSearch"></param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> SearchMovie([FromBody] ReleaseDateSearch releaseDateSearch)
        {
            var releaseDateFrom = releaseDateSearch.ReleaseDateFrom;
            var releaseDateTo = releaseDateSearch.ReleaseDateTo;

            if (releaseDateTo < releaseDateFrom)
                return BadRequest("release date to must be less than release from.");

            var result = _context.movies.Include(m => m.Bookings)
                                        .Where(m => m.ReleaseDate >= releaseDateFrom && m.ReleaseDate <= releaseDateTo).ToList();

            var movies = GetMoviesDTO(result);

            return Ok(movies);
        }

        /// <summary>
        /// /api/movies/search/{title}
        /// </summary>
        /// <param name="movieTitle"></param>
        /// <returns></returns>
        [HttpGet("search/{movieTitle}")]
        public async Task<ActionResult<MovieDTO>> SearchMovieByName(string movieTitle)
        {
            if (string.IsNullOrWhiteSpace(movieTitle))
            {
                return BadRequest("movie title is not valid.");
            }

            var result = _context.movies.Include(m => m.Bookings).Where(m => m.Title == movieTitle).FirstOrDefaultAsync().Result;

            if (result == null)
            {
                return NotFound("There is no movie with this title.");
            }

            var bookingLst = new List<BookingDTO>();

            if (result.Bookings != null)
            {
                foreach (var booking in result.Bookings)
                {
                    bookingLst.Add(new BookingDTO { Id = booking.Id, BookingTime = booking.BookingTime, UserId = booking.UserId });
                }
            }

            var movieDTO = new MovieDTO { Id = result.Id, Title = result.Title, Description = result.Description, ReleaseDate = result.ReleaseDate, Bookings = bookingLst };

            return Ok(movieDTO);
        }




        private bool MovieExists(int id)
        {
            return _context.movies.Any(e => e.Id == id);
        }

        private static List<MovieDTO> GetMoviesDTO(List<Movie> result) {
            var bookings = new List<BookingDTO>();
            var movies = new List<MovieDTO>();


            foreach (var booking in result)
            {
                bookings = new List<BookingDTO>();
                foreach (var bookingItem in booking.Bookings)
                {
                    bookings.Add(new BookingDTO { Id = bookingItem.Id, BookingTime = bookingItem.BookingTime, MovieId = bookingItem.MovieId, UserId = bookingItem.UserId });
                }

                movies.Add(new MovieDTO { Id = booking.Id, Title = booking.Title, Description = booking.Description, ReleaseDate = booking.ReleaseDate, Bookings = bookings });
            }

            return movies;
        }
    }
}
