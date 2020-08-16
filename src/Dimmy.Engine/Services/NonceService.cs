using System.Security.Cryptography;

namespace Dimmy.Engine.Services
{
    public class NonceService
    {
        public static string Generate(int length = 64)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            using var crypto = new RNGCryptoServiceProvider();
            var data = new byte[length];
            byte[] smallBuffer = null;

            var maxRandom = byte.MaxValue - (byte.MaxValue + 1) % chars.Length;

            crypto.GetBytes(data);

            var result = new char[length];

            for (var i = 0; i < length; i++)
            {
                var v = data[i];

                while (v > maxRandom)
                {
                    smallBuffer ??= new byte[1];

                    crypto.GetBytes(smallBuffer);
                    v = smallBuffer[0];
                }

                result[i] = chars[v % chars.Length];
            }

            return new string(result);
        }
    }
}