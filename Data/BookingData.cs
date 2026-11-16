using PracticeApi.Models;

namespace PracticeApi.Data
{
    public static class BookingData
    {
        public static readonly object Sync = new object();
        public static int NextId = 1;
        public static List<Booking> Bookings = new List<Booking>();
        public static List<Resource> Resources = new List<Resource>
        {
            new Resource { Id = 1, Name = "Номер 101", Type = "hotel" },
            new Resource { Id = 2, Name = "Номер 102", Type = "hotel" },
            new Resource { Id = 3, Name = "Стол 1", Type = "restaurant" }
        };
    }
}
