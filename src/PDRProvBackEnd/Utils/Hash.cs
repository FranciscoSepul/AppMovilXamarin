using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;

namespace PDRProvBackEnd.Utils
{
    public class Hash
    {
        public static string CreateHash(string content)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(content)));
                return hash;
            }
        }
        public static string CreateHash(byte[] content)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = Convert.ToBase64String(sha1.ComputeHash(content));
                return hash;
            }
        }
    }
}
