using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TokenIdentification.Client
{
    class Program
    {
        private const string _alg = "HmacSHA256";
        private const string _salt = "rz8LuOtFBXphj9WQfvFh";
        private const string userSecret = "VGhpcyBDb3Vyc2UgSXMgQXdlc29tZQ==";
        private const string userApiKey = "SSB3aWxsIG1ha2UgbXkgQVBJIHNlY3VyZQ==";
        static void Main(string[] args)
        {
            string secret = string.Empty;
            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                hmac.Key = Encoding.UTF8.GetBytes(_salt);
                var hash1 = hmac.ComputeHash(Encoding.UTF8.GetBytes(_salt));
                var hash2 = hmac.ComputeHash(Encoding.UTF8.GetBytes(userSecret));
                var hash3 = hmac.ComputeHash(Encoding.UTF8.GetBytes(userApiKey));
                int i = 0;
                byte c;
                var signature = "";
                foreach (byte b in hash2)
                {

                    c = hash1[i];
                    signature = signature + (b & hash1[i] ^hash3[i]);
                    i++;
                }
                secret = Convert.ToBase64String(Encoding.UTF8.GetBytes(signature));

            }
            Console.WriteLine(secret);
            Console.ReadKey();
        }
    }
}
