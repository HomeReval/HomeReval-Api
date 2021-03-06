﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Models.Form;
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
        private readonly IJwtHandler _jwtHandler;

        public UserController(IHttpContextAccessor httpContextAccessor, IUserService userService, IJwtHandler jwtHandler)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _jwtHandler = jwtHandler;
        }

        // Returns current user
        [HttpGet]
        public IActionResult Get()
        {
            var user_ID = _jwtHandler.GetUserID(_httpContextAccessor.HttpContext);
            return Ok(_userService.Get(user_ID));
        }

        // Login with email & password, returns access, refresh and access expiration details
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult SignIn([FromBody] SignIn request)
        {
            if (ModelState.IsValid){
                return Ok(_userService.SignIn(request.Username, request.Password));
            }
            return BadRequest(ModelState);              
        }

        // Create a new account
        [HttpPost("create")]
        [AllowAnonymous]
        public IActionResult SignUp([FromBody] SignUp signUp)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = new User
            {
                Email = signUp.Email,
                Password = signUp.Password,
                FirstName = signUp.FirstName,
                LastName = signUp.LastName,
                Gender = signUp.Gender              
            };

            _userService.SignUp(user);
            return NoContent();
        }

        // Return a new access token with the supplied Refresh Token
        [HttpPost("token/refresh")]
        [AllowAnonymous]
        public IActionResult RefreshAccessToken([FromBody] RefreshToken refreshToken)
            => Ok(_userService.RefreshAccessToken(refreshToken.Token));

    }
}