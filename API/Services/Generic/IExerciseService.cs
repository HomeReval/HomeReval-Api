using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IExerciseService
    {
        object Get(long User_ID);
        void Add(object o);
        void Update(object o);
        void Delete(object o);
        string Compress(byte[] data);
        byte[] Compress(string data);
    }
}
