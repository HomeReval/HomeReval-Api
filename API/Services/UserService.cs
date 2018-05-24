using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using API.Models;
using API.Models.Tokens;
using API.Services.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IJwtHandler _jwtHandler;
        private readonly IEncryptionManager _encryptionManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        

        public UserService(IServiceScopeFactory scopeFactory, IJwtHandler jwtHandler, 
            IEncryptionManager encryptionManager, IPasswordHasher<User> passwordHasher)
        {
            _scopeFactory = scopeFactory;
            _jwtHandler = jwtHandler;
            _encryptionManager = encryptionManager;
            _passwordHasher = passwordHasher;
        }

        //public void SignUp(string username, string password)
        //{
        //    if (string.IsNullOrWhiteSpace(username))
        //    {
        //        throw new Exception($"Username can not be empty.");
        //    }
        //    if (string.IsNullOrWhiteSpace(password))
        //    {
        //        throw new Exception($"Password can not be empty.");
        //    }
        //    if (GetUser(username) != null)
        //    {
        //        throw new Exception($"Username '{username}' is already in use.");
        //    }
        //    _users.Add(new User { Username = username, Password = password } );
        //}

        public User Get(string token)
        {
            try
            {
                var jwtToken = new JwtSecurityToken(token);
                long ID = Convert.ToInt64(jwtToken.Subject);
                return GetUser(ID);

            }
            catch (Exception e)
            {
                throw new Exception("Invalid token supplied");
            }
        }

        public JsonWebToken SignIn(string username, string password)
        {
            var user = GetUser(username);
            if (user == null)
            {
                throw new Exception("No user found");
            }

            if (!_encryptionManager.Compare(password, user.Password))
            {
                throw new Exception("Invalid credentials");
            }

            var jwt = _jwtHandler.Create(user.ID);
            var refreshToken = _passwordHasher.HashPassword(user, Guid.NewGuid().ToString())
                .Replace("+", string.Empty)
                .Replace("=", string.Empty)
                .Replace("/", string.Empty);

            var refresh = GetRefreshToken(user.ID);
            // Does the user already have a valid refresh token?
            if (refresh == null)
            {
                jwt.RefreshToken = refreshToken;

                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                    dbContext.RefreshTokens.Add(new RefreshToken { User_ID = user.ID, Token = refreshToken });
                    dbContext.SaveChanges();
                }

            } else
            {
                jwt.RefreshToken = refresh.Token;
            }

            

            return jwt;
        }

        public JsonWebToken RefreshAccessToken(string token)
        {
            var refreshToken = GetRefreshToken(token);
            if (refreshToken == null)
            {
                throw new Exception("Refresh token was not found.");
            }
            if (refreshToken.Revoked)
            {
                throw new Exception("Refresh token was revoked");
            }
            var jwt = _jwtHandler.Create(refreshToken.User_ID);;
            jwt.RefreshToken = refreshToken.Token;

            return jwt;
        }

        public void RevokeRefreshToken(string token)
        {
            var refreshToken = GetRefreshToken(token);
            if (refreshToken == null)
            {
                throw new Exception("Refresh token was not found.");
            }
            if (refreshToken.Revoked)
            {
                throw new Exception("Refresh token was already revoked.");
            }
            refreshToken.Revoked = true;
        }

        private User GetUser(string username)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                return dbContext.Users.SingleOrDefault(x => string.Equals(x.Email, username, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        private User GetUser(long User_ID)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                return dbContext.Users
                    .Include(x => x.UserGroup)
                    .SingleOrDefault(x => x.ID == User_ID);
            }
        }

        private RefreshToken GetRefreshToken(long User_ID)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                return dbContext.RefreshTokens.SingleOrDefault(x => x.User_ID == User_ID && x.Revoked != true);
            }
        }

        private RefreshToken GetRefreshToken(string token)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
                return dbContext.RefreshTokens.SingleOrDefault(x => x.Token == token && x.Revoked != true);
            }
        }

    }

}