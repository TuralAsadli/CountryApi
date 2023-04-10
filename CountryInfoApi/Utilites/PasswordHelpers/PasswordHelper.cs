using System.Security.Cryptography;

namespace CountryInfoApi.Utilites.PasswordHelpers
{
    public class PasswordHelper
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10000;

        public static void HashPassword(string password, out byte[] passwordHash, out byte[] passwordSlat)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSlat = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                
            }
        }
        public static bool VerifyPassword(string password, byte[] hashedPassword, byte[] saltPassword)
        {
            using (var hmac = new HMACSHA512(saltPassword))
            {
                var computetHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computetHash.SequenceEqual(hashedPassword);
            }
        }
    }
}
