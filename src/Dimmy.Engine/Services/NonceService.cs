using System;
using System.Security.Cryptography;

namespace Dimmy.Engine.Services
{
    public class NonceService
    {
        public static string Generate(int length = 64)
        {
            using (var cryptRNG = new RNGCryptoServiceProvider())
            {
                var tokenBuffer = new byte[length];
                cryptRNG.GetBytes(tokenBuffer);
                return Convert.ToBase64String(tokenBuffer);
            }
        }
    }
}
