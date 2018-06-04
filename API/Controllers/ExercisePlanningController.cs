using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Services;
using API.Services.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/[controller]")]
    public class ExercisePlanningController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IExercisePlanningService _exercisePlanningService;
        private readonly IUserGroupService _roleService;
        private readonly IUserPhysioService _userPhysioService;
        private readonly IUserExerciseService _userExerciseService;
        private readonly IJwtHandler _jwtHandler;

        public ExercisePlanningController(IHttpContextAccessor httpContextAccessor, IExercisePlanningService exercisePlanningService, IUserGroupService roleService, IUserPhysioService userPhysioService, IUserExerciseService userExerciseService, IJwtHandler jwtHandler)
        {
            _httpContextAccessor = httpContextAccessor;
            _exercisePlanningService = exercisePlanningService;
            _roleService = roleService;
            _userPhysioService = userPhysioService;
            _userExerciseService = userExerciseService;
            _jwtHandler = jwtHandler;
        }


        // Return all exerciseplannings of supplied weeknumber for token based User
        [HttpGet("{id}")]
        public IActionResult GetByID(long id)
        {
            var user_ID = _jwtHandler.GetUserID(_httpContextAccessor.HttpContext);
            return Ok(_exercisePlanningService.GetByID(user_ID, id));
        }

        // Return all exerciseplannings of this week for token based User
        [HttpGet("week")]
        public IActionResult GetByThisWeek()
        {
            var user_ID = _jwtHandler.GetUserID(_httpContextAccessor.HttpContext);
            int weekNum = Helper.GetIso8601WeekOfYear(DateTime.Now);
            return Ok(_exercisePlanningService.GetByWeek(user_ID, weekNum));
        }

        // Return all exerciseplannings of supplied weeknumber for token based User
        [HttpGet("week/{id}")]
        public IActionResult GetByWeek(int id)
        {
            var user_ID = _jwtHandler.GetUserID(_httpContextAccessor.HttpContext);
            return Ok(_exercisePlanningService.GetByWeek(user_ID, id));
        }

        // Create a new ExercisePlanning
        [HttpPost]
        public IActionResult Add([FromBody] ExercisePlanning exercisePlanning)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user_ID = _jwtHandler.GetUserID(_httpContextAccessor.HttpContext);
            //_roleService.IsUserManager(user_ID);
            _userPhysioService.IsMemberOfPhysio(user_ID, exercisePlanning.UserExercise.User_ID);

            var userExercise = _userExerciseService.Get(exercisePlanning.UserExercise.User_ID, exercisePlanning.UserExercise.Exercise_ID);
            exercisePlanning.UserExercise = userExercise;

            _exercisePlanningService.Add(exercisePlanning);
            return NoContent();
        }


    }

}