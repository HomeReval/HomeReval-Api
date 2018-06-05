using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Models.Form;
using API.Services;
using API.Services.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/[controller]")]
    public class ExerciseResultController : ControllerBase
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IExerciseResultService _exerciseResultService;
        private readonly IJwtHandler _jwtHandler;

        public ExerciseResultController(IHttpContextAccessor httpContextAccessor, IExerciseResultService exerciseResultService, IJwtHandler jwtHandler)
        {
            _httpContextAccessor = httpContextAccessor;
            _exerciseResultService = exerciseResultService;
            _jwtHandler = jwtHandler;
        }

        [HttpPost]
        public IActionResult Add([FromBody] ClientExerciseResult clientExerciseResult)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user_ID = _jwtHandler.GetUserID(_httpContextAccessor.HttpContext);
            var planning_ID = clientExerciseResult.ExercisePlanning_ID;
            byte[] Recording = Helper.Compress(clientExerciseResult.Result);

            ExerciseResult exerciseResult = new ExerciseResult
            {
                Duration = clientExerciseResult.Duration,
                Score = clientExerciseResult.Score,
                Result = Recording,
            };

            _exerciseResultService.AddByPlanningID(exerciseResult, user_ID, planning_ID);

            return NoContent();
        }


    }

}