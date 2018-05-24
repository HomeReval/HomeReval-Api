using API.Models;
using API.Models.Tokens;

namespace API.Services
{
    public interface IUserService
    {
        //void SignUp(string username, string password);
        JsonWebToken SignIn(string username, string password);
        User Get(string token);
        JsonWebToken RefreshAccessToken(string token);
        void RevokeRefreshToken(string token);
    }
}