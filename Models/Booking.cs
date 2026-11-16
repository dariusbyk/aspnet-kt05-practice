namespace PracticeApi.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
