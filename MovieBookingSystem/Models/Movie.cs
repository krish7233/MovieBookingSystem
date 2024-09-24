using System.Text.Json.Serialization;

namespace MovieBookingSystem.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }

        public virtual List<Cast> Casts { get; set; }
        
        public virtual List<Booking> Bookings { get; set; }
    }
}
