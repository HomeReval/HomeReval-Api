using API.Models;
using API.Services.Security;
using System;

namespace API.Services
{
    public class UserGroupService : IUserGroupService
    {

        private readonly IUserService _userService;
        private readonly IJwtHandler _jwtHandler;

        public UserGroupService(IUserService userService, IJwtHandler jwtHandler)
        {
            _userService = userService;
            _jwtHandler = jwtHandler;
        }


        public void Add(string token, object o)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(string token, object o)
        {
            throw new System.NotImplementedException();
        }

        public object Get(string token)
        {
            throw new System.NotImplementedException();
        }

        public void Update(string token, object o)
        {
            throw new System.NotImplementedException();
        }

        public void IsUserManager(string token, string method)
        {

            var user = _userService.GetUser(_jwtHandler.GetUserID(token));

            if (user.UserGroup.ID != Models.Type.Manager && user.UserGroup.ID != Models.Type.Administrator)
            {
                throw new UnauthorizedAccessException("User: " + user.Email + ", Role: " + user.UserGroup.Type + " , does not have access to: " + method);
            }
        }

        public void IsUserManager(User user, string method)
        {
            if (user.UserGroup.ID != Models.Type.Manager && user.UserGroup.ID != Models.Type.Administrator)
            {
                throw new UnauthorizedAccessException("User: " + user.Email + ", Role: " + user.UserGroup.Type + " , does not have access to: " + method);
            }
        }

    }
}