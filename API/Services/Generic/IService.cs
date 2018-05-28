using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IService
    {

        object Get(long ID);

        void Add(object o);

        void Update(object o);

        void Delete(object o);

    }
}
