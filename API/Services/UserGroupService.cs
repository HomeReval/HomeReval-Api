using API.Models;
using API.Services.Security;
using System;

namespace API.Services
{
    public class UserGroupService : IUserGroupService
    {

        private readonly IUserService _userService;

        public UserGroupService(IUserService userService)
        {
            _userService = userService;
        }


        public void Add(object o)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(object o)
        {
            throw new System.NotImplementedException();
        }

        public object Get(long Id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(object o)
        {
            throw new System.NotImplementedException();
        }

        public void IsUserManager(long user_ID)
        {

            var user = _userService.Get(user_ID);

            if (user.UserGroup.ID != Models.Type.Manager && user.UserGroup.ID != Models.Type.Administrator)
            {
                throw new UnauthorizedAccessException("User: " + user.Email + ", Role: " + user.UserGroup.Type + " , does not have access to this call");
            }
        }

        public void IsUserManager(User user)
        {
            if (user.UserGroup.ID != Models.Type.Manager && user.UserGroup.ID != Models.Type.Administrator)
            {
                throw new UnauthorizedAccessException("User: " + user.Email + ", Role: " + user.UserGroup.Type + " , does not have access to this call");
            }
        }

    }
}