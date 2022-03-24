using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ALMS.Service
{
    public class PasswordHasherService : IPasswordHasherService
    {
        private const string _salt = "2VbJrHnVmU+unF0jKvqfqw==";
        public void Dispose() { }

        public string HashedPassword(string password)
        {
            var salt = Convert.FromBase64String(_salt);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}