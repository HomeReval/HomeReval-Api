using API.Models;
using API.Models.Tokens;

namespace API.Services
{
    public interface IUserService
    {
        //void SignUp(string username, string password);
        JsonWebToken SignIn(string username, string password);
        void SignUp(User user);
        User Get(long ID);
        JsonWebToken RefreshAccessToken(string token);
        void RevokeRefreshToken(string token);

        User GetUser(long User_ID);
    }
}