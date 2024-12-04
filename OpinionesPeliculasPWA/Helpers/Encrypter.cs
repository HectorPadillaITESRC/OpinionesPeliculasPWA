using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpinionesPeliculasPWA.Helpers
{
     public static class Encrypter
    {
        public static string ConvertToSHA256(string str)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] hashValue;
            UTF8Encoding objUtf8 = new UTF8Encoding();
            hashValue = sha256.ComputeHash(objUtf8.GetBytes(str));
            return Convert.ToHexString(hashValue).ToLower();

        }
    }
}