namespace MovieBookingSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string UserId { get; set; }
        public DateTime BookingTime { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
