using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly Context _context;

        public UserController(Context context)
        {
            _context = context;

            if (_context.Users.Count() == 0)
            {
                //_context.Users.Add(new User { FirstName = "test" });
                _context.Add(new User { Email = "projecthomereval@gmail.com", Password = "default", FirstName = "Admin", LastName = "Admin", Gender = 'm', UserGroup = _context.UserGroups.Find(API.Models.Type.Administrator) });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        [HttpGet("{id}", Name = "GetAccount")]
        public IActionResult GetById(long id)
        {
            var account = _context.Users.Find(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpPost]
        public IActionResult Create([FromBody] User account)
        {
            if (account == null)
            {
                return BadRequest();
            }

            _context.Users.Add(account);
            _context.SaveChanges();

            return CreatedAtRoute("GetAccount", new { id = account.ID }, account);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] User account)
        {
            if (account == null || account.ID != id)
            {
                return BadRequest();
            }

            var todo = _context.Users.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.FirstName = account.FirstName;

            _context.Users.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var account = _context.Users.Find(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Users.Remove(account);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
