using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.DTOs;
using MovieBookingSystem.Models;
using MovieBookingSystem.Repositories;
using System.Diagnostics.Contracts;

namespace MovieBookingSystem.Services
{
    public class MovieService
    {
        private readonly MovieRepository repository;
        public MovieService(MovieRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<MovieDTO>> GetMovies() 
        {
            var movies = await repository.GetMovies();

            var moviesDto = GetMoviesDTO(movies);

            return moviesDto;
        }

        public async Task<MovieDTO> GetMovieById(int id)
        {
            var movie = await repository.GetMovieById(id);

            var bookingLst = new List<BookingDTO>();

            foreach (var booking in movie.Bookings)
            {
                bookingLst.Add(new BookingDTO { Id = booking.Id, BookingTime = booking.BookingTime, UserId = booking.UserId });
            }

            var castLst = new List<CastDTO>();

            foreach (var cast in movie.Casts)
            {
                castLst.Add(new CastDTO { Id = cast.Id, Description = cast.Description, Name = cast.Name });
            }

            var movieDTO = new MovieDTO { Id = movie.Id, Title = movie.Title, Description = movie.Description, ReleaseDate = movie.ReleaseDate, Bookings = bookingLst, casts = castLst };

            return movieDTO;
        }

        private static List<MovieDTO> GetMoviesDTO(List<Movie> result)
        {
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

        public async Task UpdateMovie(Movie movie)
        {
            await repository.UpdateMovie(movie);
        }

        public bool MovieExists(int id) 
        {
            return repository.MovieExists(id);
        }

        public async Task<Movie> SaveMovie(MovieDTO movieDTO)
        {
            var movie = GetMovie(movieDTO);

            return await repository.SaveMovie(movie);
        }

        private Movie GetMovie(MovieDTO movieDTO)
        {
            var movieCasts = new List<Cast>();
            var movieBookings = new List<Booking>();

            foreach (var cast in movieDTO.casts)
            {
                movieCasts.Add(new Cast { Id = cast.Id, Name = cast.Name, Description = cast.Description });
            }

            foreach (var booking in movieDTO.Bookings)
            {
                movieBookings.Add(new Booking { Id = booking.Id, UserId = booking.UserId, BookingTime = booking.BookingTime });
            }

            var movie = new Movie() { Title = movieDTO.Title, Description = movieDTO.Description, ReleaseDate = movieDTO.ReleaseDate, Casts = movieCasts, Bookings = movieBookings };

            return movie;
        }

        public async Task DeleteMovie(MovieDTO movieDto)
        {
            var movie = GetMovie(movieDto);

            await repository.DeleteMovie(movie);
        }
    }
}
