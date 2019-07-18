using System;
using System.Security.Cryptography;
using System.Text;

namespace Services.Helpers
{
    public class Encryption
    {
        public string HashHmac(string secret, string password)
        {
            Encoding encoding = Encoding.UTF8;
            using (HMACSHA512 hmac = new HMACSHA512(encoding.GetBytes(password)))
            {
                var key = encoding.GetBytes(secret);
                var hash = hmac.ComputeHash(key);
                return BitConverter.ToString(hash).ToLower().Replace("-", string.Empty);
            }
        }
    }
}
