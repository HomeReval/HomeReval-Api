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
    public class ExercisePlanningPlanningController : ControllerBase
    {
        private readonly Context _context;

        public ExercisePlanningPlanningController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public List<ExercisePlanning> GetAll()
        {
            return _context.ExercisePlannings
                //.Include(p => p.UserGroup)
                .ToList();
        }

        [HttpGet("{id}", Name = "GetExercisePlanning")]
        public IActionResult GetById(long id)
        {
            var exercisePlanning = _context.ExercisePlannings.Find(id);
            if (exercisePlanning == null)
            {
                return NotFound();
            }
            return Ok(exercisePlanning);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ExercisePlanning exercisePlanning)
        {
            if (exercisePlanning == null)
            {
                return BadRequest();
            }


            _context.ExercisePlannings.Add(exercisePlanning);
            _context.SaveChanges();

            return CreatedAtRoute("GetExercisePlanning", new { id = exercisePlanning.ID }, exercisePlanning);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] ExercisePlanning exercisePlanning)
        {
            if (exercisePlanning == null || exercisePlanning.ID != id)
            {
                return BadRequest();
            }

            var todo = _context.ExercisePlannings.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            // Update Logic

            _context.ExercisePlannings.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var user = _context.ExercisePlannings.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.ExercisePlannings.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }

    }

}