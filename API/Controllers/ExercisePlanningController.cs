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
        private readonly IJwtHandler _jwtHandler;

        public ExercisePlanningController(IHttpContextAccessor httpContextAccessor, IExercisePlanningService exercisePlanningService, IUserGroupService roleService, IJwtHandler jwtHandler)
        {
            _httpContextAccessor = httpContextAccessor;
            _exercisePlanningService = exercisePlanningService;
            _roleService = roleService;
            _jwtHandler = jwtHandler;
        }

        [HttpGet("week")]
        public IActionResult GetByThisWeek()
        {
            var token = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"]
                .Single()
                .Split(" ")
                .Last();

            var ID = _jwtHandler.GetUserID(token);

            int weekNum = Helper.GetIso8601WeekOfYear(DateTime.Now);
            return Ok(_exercisePlanningService.GetByWeek(ID, weekNum));
        }

        [HttpGet("week/{id}")]
        public IActionResult GetByWeek(int id)
        {
            var token = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"]
                .Single()
                .Split(" ")
                .Last();

            var ID = _jwtHandler.GetUserID(token);
            return Ok(_exercisePlanningService.GetByWeek(ID, id));
        }



    }

}