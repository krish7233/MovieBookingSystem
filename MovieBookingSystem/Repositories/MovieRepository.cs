using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.AppDBContexts;
using MovieBookingSystem.DTOs;
using MovieBookingSystem.Models;

namespace MovieBookingSystem.Repositories
{
    public class MovieRepository
    {
        private readonly MovieBookingDBContext dbContext;

        public MovieRepository(MovieBookingDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Movie>> GetMovies(){
            var movies = dbContext.movies.Include(m => m.Bookings).ToListAsync().Result;

            return movies;
        }

        public async Task<Movie> GetMovieById(int id)
        {
            //throw new NotImplementedException();

            return await dbContext.movies.Include(m => m.Bookings).Include(m => m.Casts).FirstOrDefaultAsync(m => m.Id == id); ;
        }

        public async Task UpdateMovie(Movie movie) 
        {
            dbContext.Entry(movie).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<Movie> SaveMovie(Movie movie)
        {
            dbContext.movies.Add(movie);

            try
            {
                await dbContext.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException) 
            {
                throw;
            }

            return movie;
        }


        public async Task DeleteMovie(Movie movie)
        {
            dbContext.movies.Remove(movie);

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) 
            {
                throw;
            }
        }


        public bool MovieExists(int id) 
        {
            return dbContext.movies.Any(e => e.Id == id);
        }
    }
}
