using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IExerciseService : IService
    {
        string Compress(byte[] data);
        byte[] Compress(string data);
        object GetByUserID(long ID);
    }
}
