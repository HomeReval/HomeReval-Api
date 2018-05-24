using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Models.Tokens;
using API.Services;
using API.Services.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// Token information: https://stackoverflow.com/questions/40281050/jwt-authentication-for-asp-net-web-api

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly ITokenManager _tokenManager;

        public UserController(IHttpContextAccessor httpContextAccessor, IUserService userService, ITokenManager tokenManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _tokenManager = tokenManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var token = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"]
                .Single()
                .Split(" ")
                .Last();

            return Ok(_userService.Get(token));

        }
             

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult SignIn([FromBody] SignIn request)
            => Ok(_userService.SignIn(request.Username, request.Password));

        [HttpPost("create")]
        [AllowAnonymous]
        public IActionResult SignUp([FromBody] User user)
        {
            _userService.SignUp(user);
            return NoContent();
        }

        [HttpPost("token/refresh")]
        [AllowAnonymous]
        public IActionResult RefreshAccessToken2([FromBody] RefreshToken refreshToken)
            => Ok(_userService.RefreshAccessToken(refreshToken.Token));


        //[HttpGet("{id}", Name = "GetUser")]
        //public IActionResult GetById(long id)
        //{
        //    var user = _context.Users
        //        .Include(u => u.UserGroup)
        //        .SingleOrDefault(x => x.ID == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(user);
        //}

        //[HttpPost]
        //public IActionResult Create([FromBody] User user)
        //{
        //    if (user == null)
        //    {
        //        return BadRequest();
        //    }

        //    user.Password = _encryptionManager.Encrypt(user.Password);

        //    _context.Users.Add(user);
        //    _context.SaveChanges();

        //    return CreatedAtRoute("GetUser", new { id = user.ID }, user);
        //}

        //[HttpPut("{id}")]
        //public IActionResult Update(long id, [FromBody] User user)
        //{
        //    if (user == null || user.ID != id)
        //    {
        //        return BadRequest();
        //    }

        //    var todo = _context.Users.Find(id);
        //    if (todo == null)
        //    {
        //        return NotFound();
        //    }

        //    todo.FirstName = user.FirstName;

        //    _context.Users.Update(todo);
        //    _context.SaveChanges();
        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public IActionResult Delete(long id)
        //{
        //    var user = _context.Users.Find(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    _context.SaveChanges();
        //    return NoContent();
        //}


    }
}

//namespace TokenManager.Api.Controllers
//{
//    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//    public class AccountController : Controller
//    {
//        private readonly IAccountService _accountService;
//        private readonly ITokenManager _tokenManager;

//        public AccountController(IAccountService accountService,
//                ITokenManager tokenManager)
//        {
//            _accountService = accountService;
//            _tokenManager = tokenManager;
//        }

//        [HttpGet("account")]
//        public IActionResult Get([FromBody] SignUp request)
//            => Content($"Hello {User.Identity.Name}");

//        [HttpPost("sign-up")]
//        [AllowAnonymous]
//        public IActionResult SignUp([FromBody] SignUp request)
//        {
//            _accountService.SignUp(request.Username, request.Password);

//            return NoContent();
//        }

//        [HttpPost("sign-in")]
//        [AllowAnonymous]
//        public IActionResult SignIn([FromBody] SignIn request)
//            => Ok(_accountService.SignIn(request.Username, request.Password));

//        [HttpPost("tokens/{token}/refresh")]
//        [AllowAnonymous]
//        public IActionResult RefreshAccessToken(string token)
//            => Ok(_accountService.RefreshAccessToken(token));

//        [HttpPost("tokens/{token}/revoke")]
//        public IActionResult RevokeRefreshToken(string token)
//        {
//            _accountService.RevokeRefreshToken(token);

//            return NoContent();
//        }

//        [HttpPost("tokens/cancel")]
//        public async Task<IActionResult> CancelAccessToken()
//        {
//            await _tokenManager.DeactivateCurrentAsync();

//            return NoContent();
//        }
//    }
//}