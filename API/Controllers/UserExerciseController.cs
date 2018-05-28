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
    public class UserExerciseController : ControllerBase
    {
        private readonly Context _context;

        public UserExerciseController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public List<UserExercise> GetAll()
        {
            return _context.UserExercises
                //.Include(p => p.UserGroup)
                .ToList();
        }
        [HttpGet("")]

        [HttpGet("{id}", Name = "GetUserUserExercise")]
        public IActionResult GetById(long id)
        {
            var userExercise = _context.UserExercises.Find(id);
            if (userExercise == null)
            {
                return NotFound();
            }
            return Ok(userExercise);
        }

        [HttpPost]
        public IActionResult Create([FromBody] UserExercise userExercise)
        {
            if (userExercise == null)
            {
                return BadRequest();
            }


            _context.UserExercises.Add(userExercise);
            _context.SaveChanges();

            return CreatedAtRoute("GetUserExercise", new { id = userExercise.ID }, userExercise);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] UserExercise userExercise)
        {
            if (userExercise == null || userExercise.ID != id)
            {
                return BadRequest();
            }

            var todo = _context.UserExercises.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            // Update Logic

            _context.UserExercises.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var user = _context.UserExercises.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.UserExercises.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }

    }

}