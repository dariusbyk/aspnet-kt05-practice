using Microsoft.AspNetCore.Mvc;
using PracticeApi.Data;
using PracticeApi.Models;

namespace PracticeApi.Controllers
{
    [ApiController]
    [Route("bookings")]
    public class BookingsController : ControllerBase
    {
        [HttpPost]
        public IActionResult Create(Booking booking)
        {
            if (booking.Start >= booking.End)
            {
                return BadRequest("Начало должно быть раньше окончания.");
            }

            lock (BookingData.Sync)
            {
                Resource resource = BookingData.Resources.FirstOrDefault(r => r.Id == booking.ResourceId);
                if (resource == null)
                {
                    return NotFound("Ресурс не найден.");
                }

                bool occupied = BookingData.Bookings.Any(b =>
                    b.ResourceId == booking.ResourceId && b.Start < booking.End && b.End > booking.Start);
                if (occupied)
                {
                    return Conflict("Ресурс занят в это время.");
                }

                booking.Id = BookingData.NextId;
                BookingData.NextId++;
                BookingData.Bookings.Add(booking);
                return StatusCode(201, booking);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Booking input)
        {
            if (input.Start >= input.End)
            {
                return BadRequest("Начало должно быть раньше окончания.");
            }

            lock (BookingData.Sync)
            {
                Booking booking = BookingData.Bookings.FirstOrDefault(b => b.Id == id);
                if (booking == null)
                {
                    return NotFound("Бронирование не найдено.");
                }

                Resource resource = BookingData.Resources.FirstOrDefault(r => r.Id == input.ResourceId);
                if (resource == null)
                {
                    return NotFound("Ресурс не найден.");
                }

                bool occupied = BookingData.Bookings.Any(b =>
                    b.Id != id && b.ResourceId == input.ResourceId &&
                    b.Start < input.End && b.End > input.Start);
                if (occupied)
                {
                    return Conflict("Ресурс занят в это время.");
                }

                booking.ResourceId = input.ResourceId;
                booking.Start = input.Start;
                booking.End = input.End;
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Cancel(int id)
        {
            lock (BookingData.Sync)
            {
                Booking booking = BookingData.Bookings.FirstOrDefault(b => b.Id == id);
                if (booking == null)
                {
                    return NotFound();
                }
                BookingData.Bookings.Remove(booking);
                return NoContent();
            }
        }
    }
}
