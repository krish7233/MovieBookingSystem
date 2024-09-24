namespace MovieBookingSystem.DTOs
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string UserId { get; set; }
        public DateTime BookingTime { get; set; }
    }
}
