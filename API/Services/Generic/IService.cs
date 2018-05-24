using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IService
    {

        object Get(string token);

        void Add(string token, object o);

        void Update(string token, object o);

        void Delete(string token, object o);

    }
}
