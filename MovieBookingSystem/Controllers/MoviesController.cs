using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using MovieBookingSystem.Services;

namespace MovieBookingSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieService service;

        public MoviesController(MovieService service)
        {
            this.service = service;
        }

        // GET: api/Movies
        [Authorize(Roles = "user,admin")]
        [HttpGet]
        public async Task<ActionResult<List<MovieDTO>>> Getmovies()
        {
            var result = await service.GetMovies();
                        
            return result;
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
            
            var movieDTO = await service.GetMovieById(id);

            if (movieDTO == null)
            {
                return NotFound();
            }
            
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

            if (!service.MovieExists(id))
            {
                return NotFound();
            }

            try
            {
                await service.UpdateMovie(movie);
            }
            catch (Exception ex) 
            { 
                Debug.WriteLine("An unexpected error occurs while updating movie => " + ex.Message);
            }



            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(MovieDTO movieDTO)
        {
            var movie = new Movie();

            try
            {
                movie = await service.SaveMovie(movieDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movieDTO = await service.GetMovieById(id);

            if (movieDTO == null)
            {
                return NotFound();
            }

            await service.DeleteMovie(movieDTO);

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


    }
}
