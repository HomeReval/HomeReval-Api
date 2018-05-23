using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly Context _context;

        public ExerciseController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public List<Exercise> GetAll()
        {
            return _context.Exercises
                //.Include(p => p.UserGroup)
                .ToList();
        }

        [HttpGet("{id}", Name = "GetExercise")]
        public IActionResult GetById(long id)
        {
            var exercise = _context.Exercises.Find(id);
            if (exercise == null)
            {
                return NotFound();
            }
            return Ok(exercise);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Exercise exercise)
        {
            if (exercise == null)
            {
                return BadRequest();
            }


            _context.Exercises.Add(exercise);
            _context.SaveChanges();

            return CreatedAtRoute("GetExercise", new { id = exercise.ID }, exercise);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Exercise exercise)
        {
            if (exercise == null || exercise.ID != id)
            {
                return BadRequest();
            }

            var todo = _context.Exercises.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            // Update Logic

            _context.Exercises.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var user = _context.Exercises.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Exercises.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }

    }

}