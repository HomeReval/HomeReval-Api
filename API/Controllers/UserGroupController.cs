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
    public class UserGroupController : ControllerBase
    {
        private readonly Context _context;

        public UserGroupController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public List<UserGroup> GetAll()
        {
            return _context.UserGroups.ToList();
        }

        [HttpGet("{id}", Name = "GetUserGroup")]
        public IActionResult GetById(long id)
        {
            var userGroup = _context.UserGroups.Find(id);
            if (userGroup == null)
            {
                return NotFound();
            }
            return Ok(userGroup);
        }

    }
}
