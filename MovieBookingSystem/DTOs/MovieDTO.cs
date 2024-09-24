using MovieBookingSystem.Models;

namespace MovieBookingSystem.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }

        public List<CastDTO> casts { get; set; }

        public List<BookingDTO> Bookings { get; set; }
    }
}
