using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IUserGroupService : IService
    {

        void IsUserManager(long user_ID);
        void IsUserManager(User user);

    }
}
