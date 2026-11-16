using Microsoft.AspNetCore.Mvc;
using PracticeApi.Models;

namespace PracticeApi.Controllers
{
    [ApiController]
    [Route("students")]
    public class StudentsController : ControllerBase
    {
        private static List<Student> students = new List<Student>();
        private static int nextId = 1;

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(students);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Student student = students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {
            student.Id = nextId;
            nextId++;
            students.Add(student);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Student input)
        {
            Student student = students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            student.Name = input.Name;
            student.Age = input.Age;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Student student = students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            students.Remove(student);
            return NoContent();
        }
    }
}
