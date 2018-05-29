using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Models.Form;
using API.Services;
using API.Services.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ExerciseController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IExerciseService _exerciseService;
        private readonly IUserGroupService _roleService;
        private readonly IJwtHandler _jwtHandler;

        public ExerciseController(IHttpContextAccessor httpContextAccessor, IExerciseService exerciseService, IUserGroupService roleService, IJwtHandler jwtHandler)
        {
            _httpContextAccessor = httpContextAccessor;
            _exerciseService = exerciseService;
            _roleService = roleService;
            _jwtHandler = jwtHandler;
        }

        [Route("week")]
        [HttpGet("{id}")]
        public IActionResult GetByWeek(int weeknumber)
        {
            var token = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"]
                .Single()
                .Split(" ")
                .Last();

            var ID = _jwtHandler.GetUserID(token);
            return Ok(_exerciseService.GetByWeek(ID, weeknumber));
        }

        [HttpGet]
        public IActionResult GetById()
        {
            var token = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"]
                .Single()
                .Split(" ")
                .Last();

            var ID = _jwtHandler.GetUserID(token);
            return Ok(_exerciseService.GetByUserID(ID));
        }

        [HttpPost]
        public IActionResult Add([FromBody] ClientExercise clientExercise)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"]
                .Single()
                .Split(" ")
                .Last();

            _roleService.IsUserManager(token, "Add Exercise");

            byte[] Recording = _exerciseService.Compress(clientExercise.Recording);
            _exerciseService.Add(new Exercise { Name = clientExercise.Name, Description = clientExercise.Description, Recording = Recording });
            return NoContent();
        }
    }
}