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
    public class AccountController : ControllerBase
    {
        private readonly AccountContext _context;

        public AccountController(AccountContext context)
        {
            _context = context;

            if (_context.Accounts.Count() == 0)
            {
                _context.Accounts.Add(new Account { Name = "test" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public List<Account> GetAll()
        {
            return _context.Accounts.ToList();
        }

        [HttpGet("{id}", Name = "GetAccount")]
        public IActionResult GetById(long id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Account account)
        {
            if (account == null)
            {
                return BadRequest();
            }

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return CreatedAtRoute("GetAccount", new { id = account.Id }, account);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Account account)
        {
            if (account == null || account.Id != id)
            {
                return BadRequest();
            }

            var todo = _context.Accounts.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.Name = account.Name;

            _context.Accounts.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
