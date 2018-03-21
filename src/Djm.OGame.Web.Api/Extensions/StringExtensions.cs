using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace Djm.OGame.Web.Api.Helpers
{
    public static class StringExtensions
    {
        public static string ToHash(this string password,byte[] salt,out byte[] generatedSalt)
        {
            if (salt == null)
            {
                salt = new byte[128 / 8];

                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

            }
              
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
                
            generatedSalt = salt;
            return hashed;
        }
    }
}