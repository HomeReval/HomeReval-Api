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
    public class ExerciseResultResultController : ControllerBase
    {
        private readonly Context _context;

        public ExerciseResultResultController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public List<ExerciseResult> GetAll()
        {
            return _context.ExerciseResults
                //.Include(p => p.UserGroup)
                .ToList();
        }

        [HttpGet("{id}", Name = "GetExerciseResult")]
        public IActionResult GetById(long id)
        {
            var exerciseResult = _context.ExerciseResults.Find(id);
            if (exerciseResult == null)
            {
                return NotFound();
            }
            return Ok(exerciseResult);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ExerciseResult exerciseResult)
        {
            if (exerciseResult == null)
            {
                return BadRequest();
            }


            _context.ExerciseResults.Add(exerciseResult);
            _context.SaveChanges();

            return CreatedAtRoute("GetExerciseResult", new { id = exerciseResult.ID }, exerciseResult);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] ExerciseResult exerciseResult)
        {
            if (exerciseResult == null || exerciseResult.ID != id)
            {
                return BadRequest();
            }

            var todo = _context.ExerciseResults.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            // Update Logic

            _context.ExerciseResults.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var user = _context.ExerciseResults.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.ExerciseResults.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }

    }

}