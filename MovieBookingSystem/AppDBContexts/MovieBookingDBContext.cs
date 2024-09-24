using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.Models;

namespace MovieBookingSystem.AppDBContexts
{
    public class MovieBookingDBContext : DbContext
    {
        public MovieBookingDBContext(DbContextOptions<MovieBookingDBContext> options): base(options) 
        { 
        
        }

        public DbSet<Movie> movies { get; set; }
        public DbSet<Booking> bookings { get; set; }
        public DbSet<User> users { get; set; }

        public DbSet<Cast> Casts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>()
                .HasOne(p => p.Movie)
                .WithMany(p => p.Bookings)
                .HasForeignKey(k => k.MovieId)
                .IsRequired(false);
        }
    }
}
