using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IUserGroupService : IService
    {

        void IsUserManager(string token, string method);
        void IsUserManager(User user, string method);

    }
}
