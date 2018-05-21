using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Security
{
    // glhf with decrypting this <3
    public class Encryption
    {

        private const int Iterations = 10000; // The more iterations the securer the password is encrypted but the slower it goes (100 = 20ms, 1000 = 40ms, 10000 = 200ms)
        private const int SaltBytes = 16;
        private const int HashBytes = 20;
        private const int ByteSize = SaltBytes + HashBytes; // NOTE: ByteSize / 2 >= SaltBytes
        private const bool Debug = false;

        // Define the position of the salt bytes within the final HashBytes
        // An attacker can only know the correct salt byte position if he knows the hash unique to the given password
        // GetMap2 had 255 possibilities of salt arrangement. This one as 2^36 different possibilities
        public static byte[] GetMap(string password)
        {

            Predicate<int> isEven = delegate (int x) { return x % 2 == 0; };
            bool even = true;

            var pbkdf2 = new Rfc2898DeriveBytes(password, new byte[8], 100);
            byte[] hash = pbkdf2.GetBytes(ByteSize); // get a bytesize hash of bytes

            // Are there enough uneven numbers?
            if (hash.Where(x => x % 2 == 1).ToArray().Length > SaltBytes)
                even = false;

            byte[] map = new byte[ByteSize];
            int saltAmount = 0;

            for (int i = 0; i < ByteSize; i++)
            {

                // if there are enough even numbers to fill in the required SaltBytes
                if (even)
                {

                    if (isEven(hash[i]) && !(saltAmount == SaltBytes))
                    {

                        map[i] = 1;
                        saltAmount++;

                    }
                    else
                    {
                        map[i] = 0;
                    }
                    continue;
                }

                if (!(isEven(hash[i])) && !(saltAmount == SaltBytes))
                {
                    map[i] = 1;
                    saltAmount++;
                }
                else
                {
                    map[i] = 0;
                }

            }

            if (ValidateMap(map))
            {
                return map;
            }

            throw new InvalidOperationException("The map is not valid!");

        }

        // Place the salt and hash in correct position based on the map
        private static byte[] GetHashBytes(byte[] map, byte[] hash, byte[] salt)
        {

            //var saltbytes = map.Where(x => x == 1).ToArray();

            byte[] hashBytes = new byte[ByteSize];

            int i = 0;
            foreach (byte b in map)
            {

                if (map[i] == 0)
                {
                    hashBytes[i] = hash.First();
                    hash = hash.Skip(1).ToArray();
                }
                else
                {
                    hashBytes[i] = salt.First();
                    salt = salt.Skip(1).ToArray();
                }
                i++;

            }

            return hashBytes;

        }

        // Get the salt based on the password and hashedPassword
        private static byte[] GetSalt(string password, byte[] hashedPassword)
        {

            // 0, 0, 1, 1, 0, 0, 1
            byte[] mapping = GetMap(password); //36
            List<byte> salt = new List<byte>(); // 16

            int i = 0;
            foreach (byte b in mapping)
            {

                if (mapping[i] == 1)
                {
                    salt.Add(hashedPassword[i]);
                }
                i++;
            }

            return salt.ToArray();


        }

        // Get the hash based on the password and hashedPassword
        private static byte[] GetHash(string password, byte[] hashedPassword)
        {
            {

                // 0, 0, 1, 1, 0, 0, 1
                byte[] mapping = GetMap(password); //36
                List<byte> hash = new List<byte>(); //20

                int i = 0;
                foreach (byte b in mapping)
                {

                    if (mapping[i] == 0)
                    {
                        hash.Add(hashedPassword[i]);
                    }
                    i++;
                }

                return hash.ToArray();


            }
        }

        // Change a password one-way into a hashed string using salt
        public static string Encrypt(string password)
        {

            byte[] salt;

            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltBytes]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations + password.Length); // Not too sure about this one, could this allow an attacker to somehow backtrack how many iterations a pbkdf2 has taken?

            byte[] hash = pbkdf2.GetBytes(HashBytes);
            byte[] map = GetMap(password);
            byte[] hashBytes = GetHashBytes(map, hash, salt);

            if (Debug)
            {
                Console.WriteLine("Salt: " + PrintBytes(salt));
                Console.WriteLine("Hash: " + PrintBytes(hash));
                Console.WriteLine("Map: " + PrintBytes(map));
                Console.WriteLine("hashBytes: " + PrintBytes(hashBytes));
            }

            return Convert.ToBase64String(hashBytes);

        }

        // Compare a password to the hashedPassword
        public static bool Check(string password, string hashedPassword)
        {

            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] map = GetMap(password);
            byte[] salt = new byte[SaltBytes];

            salt = GetSalt(password, hashBytes);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations + password.Length); // Not too sure about this one, could this allow an attacker to somehow backtrack how many iterations a pbkdf2 has taken?
            byte[] hash = pbkdf2.GetBytes(HashBytes);
            byte[] compareHash = GetHash(password, hashBytes);

            if (Debug)
            {
                Console.WriteLine("Salt: " + PrintBytes(salt));
                Console.WriteLine("Map: " + PrintBytes(map));
                Console.WriteLine("Hash: " + PrintBytes(hash));
                Console.WriteLine("CompareHash: " + PrintBytes(compareHash));
            }

            for (int i = 0; i < HashBytes; i++)
                if (compareHash[i] != hash[i])
                    return false;

            return true;

        }

        // Print a byte as a string, only used for debugging 
        private static string PrintBytes(byte[] byteArray)
        {
            var sb = new StringBuilder("new byte[ByteSize] { ");
            for (var i = 0; i < byteArray.Length; i++)
            {
                var b = byteArray[i];
                sb.Append(b);
                if (i < byteArray.Length - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(" }");
            return sb.ToString();
        }

        // Method to validate if map meets HashBytes and SaltBytes requirements
        private static bool ValidateMap(byte[] map)
        {

            if (!(map.Where(x => x == 1).Count() == SaltBytes && map.Where(x => x == 0).Count() == HashBytes))
            {
                Console.WriteLine("HashBytes AND SaltBytes are invalid in this map!");
                return false;
            }
            else if (!(map.Where(x => x == 1).Count() == SaltBytes))
            {
                Console.WriteLine("SaltByes are invalid in this map!");
                return false;
            }
            else if (!(map.Where(x => x == 0).Count() == HashBytes))
            {
                Console.WriteLine("HashBytes are invalid in this map!");
                return false;
            }
            return true;

        }


        // https://medium.com/@mehanix/lets-talk-security-salted-password-hashing-in-c-5460be5c3aae orignal two methods according to this article
        private static string EncryptOld(string password)
        {
            byte[] salt;

            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);

            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];

            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            Console.WriteLine("Salt: " + PrintBytes(salt));
            Console.WriteLine("Hash: " + PrintBytes(hash));
            Console.WriteLine("hashBytes: " + PrintBytes(hashBytes));

            return Convert.ToBase64String(hashBytes);

        }

        private static bool CheckOld(string password, string hashedPassword)
        {

            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            Console.WriteLine("Salt: " + PrintBytes(salt));
            Console.WriteLine("Hash: " + PrintBytes(hash));
            Console.WriteLine("hashBytes: " + PrintBytes(hashBytes));

            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;

            return true;

        }

    }
}
