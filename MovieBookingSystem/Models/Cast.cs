namespace MovieBookingSystem.Models
{
    public class Cast
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Movie> movies { get; set; }
        public string Description { get; set; }
    }
}
