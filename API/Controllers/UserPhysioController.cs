using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    [Route("api/[controller]")]
    public class UserPhysioController : ControllerBase
    {
        private readonly Context _context;

        public UserPhysioController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public List<UserPhysio> GetAll()
        {
            return _context.UserPhysios
                //.Include(p => p.UserGroup)
                .ToList();
        }

        [HttpGet("{userID}-{physioID}", Name = "GetUserPhysio")]
        public IActionResult GetById(long userID, long physioID)
        {
            var userPhysio = _context.UserPhysios
                .Include(u => u.User)
                    .ThenInclude(u => u.UserGroup)
                .Include(p => p.Physio)
                    .ThenInclude(p => p.UserGroup)
                .SingleOrDefault( up => up.User_ID == userID && up.Physio_ID == physioID);

            if (userPhysio == null)
            {
                return NotFound();
            }
            return Ok(userPhysio);
        }

        [HttpPost]
        public IActionResult Create([FromBody] UserPhysio userPhysio)
        {
            if (userPhysio == null)
            {
                return BadRequest();
            }

            _context.UserPhysios.Add(userPhysio);
            _context.SaveChanges();

            return CreatedAtRoute("GetExercise", userPhysio);
        }

    }

}