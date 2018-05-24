using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
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
        private readonly ITokenManager _tokenManager;

        public ExerciseController(IHttpContextAccessor httpContextAccessor, IExerciseService exerciseService, ITokenManager tokenManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _exerciseService = exerciseService;
            _tokenManager = tokenManager;
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            var token = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"]
                .Single()
                .Split(" ")
                .Last();
            return Ok(_exerciseService.Get(token));
        }

    }

}