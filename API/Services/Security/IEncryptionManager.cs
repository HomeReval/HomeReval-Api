using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services.Security
{
    public interface IEncryptionManager
    {

        string Encrypt(string password);
        bool Compare(string password, string hashedPassword);

    }
}
