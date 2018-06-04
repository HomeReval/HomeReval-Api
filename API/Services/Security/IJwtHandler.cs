using API.Models.Tokens;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;

namespace API.Services.Security
{
    public interface IJwtHandler
    {
        JsonWebToken Create(long User_ID);
        long GetUserID(HttpContext httpContext);
    }
}