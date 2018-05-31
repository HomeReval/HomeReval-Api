using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IUserPhysioService : IService
    {

        void IsMemberOfPhysio(long physio_ID, long user_ID);

    }
}
