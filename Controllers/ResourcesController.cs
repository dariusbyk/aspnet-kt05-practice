using Microsoft.AspNetCore.Mvc;
using PracticeApi.Data;
using PracticeApi.Models;

namespace PracticeApi.Controllers
{
    [ApiController]
    [Route("resources")]
    public class ResourcesController : ControllerBase
    {
        [HttpGet("available")]
        public IActionResult GetAvailable(DateTime start, DateTime end)
        {
            if (start >= end)
            {
                return BadRequest("Начало должно быть раньше окончания.");
            }

            List<Resource> available = new List<Resource>();
            lock (BookingData.Sync)
            {
                foreach (Resource resource in BookingData.Resources)
                {
                    bool occupied = BookingData.Bookings.Any(b =>
                        b.ResourceId == resource.Id && b.Start < end && b.End > start);
                    if (!occupied)
                    {
                        available.Add(resource);
                    }
                }
            }
            return Ok(available);
        }
    }
}
