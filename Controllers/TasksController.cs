using Microsoft.AspNetCore.Mvc;
using PracticeApi.Models;

namespace PracticeApi.Controllers
{
    [ApiController]
    [Route("tasks")]
    public class TasksController : ControllerBase
    {
        private static List<WorkTask> tasks = new List<WorkTask>();
        private static int nextId = 1;

        [HttpGet]
        public IActionResult GetAll([FromQuery] string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return Ok(tasks);
            }
            List<WorkTask> result = tasks.Where(t => t.Status == status).ToList();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create(WorkTask task)
        {
            task.Id = nextId;
            nextId++;
            task.Status = "новая";
            tasks.Add(task);
            return StatusCode(201, task);
        }

        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(int id, StatusUpdate input)
        {
            WorkTask task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            if (input.Status != "новая" && input.Status != "в работе" && input.Status != "выполнена")
            {
                return BadRequest("Статус: новая, в работе или выполнена.");
            }
            task.Status = input.Status;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            WorkTask task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            tasks.Remove(task);
            return NoContent();
        }
    }
}
