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

        [HttpGet]
        public IActionResult GetByUserID()
        {
            var user_ID = _jwtHandler.GetUserID(_httpContextAccessor.HttpContext);
            return Ok(_exerciseService.GetByUserID(user_ID));
        }

        [HttpGet("{id}")]
        public IActionResult GetByID(long id)
        {
            var user_ID = _jwtHandler.GetUserID(_httpContextAccessor.HttpContext);
            return Ok(_exerciseService.GetByIDAndUserID(id, user_ID));

        }

        [HttpPost]
        public IActionResult Add([FromBody] ClientExercise clientExercise)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var user_ID = _jwtHandler.GetUserID(_httpContextAccessor.HttpContext);
            //_roleService.IsUserManager(user_ID);

            byte[] Recording = _exerciseService.Compress(clientExercise.Recording);
            var exercise = _exerciseService.AddWithReturn(new Exercise { Name = clientExercise.Name, Description = clientExercise.Description, Recording = Recording });
            return Ok(exercise);
        }
    }
}