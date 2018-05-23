using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Services.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// Token information: https://stackoverflow.com/questions/40281050/jwt-authentication-for-asp-net-web-api

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly Context _context;
        private readonly IEncryptionManager _encryptionManager;

        public UserController(Context context, IEncryptionManager encryptionManager)
        {
            _context = context;
            _encryptionManager = encryptionManager;
        }

        [HttpGet]
        public List<User> GetAll()
        {
            return _context.Users
                //.Include(p => p.UserGroup)
                .ToList();
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetById(long id)
        {
            var user = _context.Users
                .Include(u => u.UserGroup)
                .SingleOrDefault(x => x.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            user.Password = _encryptionManager.Encrypt(user.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtRoute("GetUser", new { id = user.ID }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] User user)
        {
            if (user == null || user.ID != id)
            {
                return BadRequest();
            }

            var todo = _context.Users.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.FirstName = user.FirstName;

            _context.Users.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
