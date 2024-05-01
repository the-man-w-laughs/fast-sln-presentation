using System.Security.Cryptography;

namespace Shared.Services.Static
{
    public static class PasswordHashingService
    {
        // Configuration parameters for hashing
        private const int SaltSize = 16; // Size of the salt in bytes (128 bits)
        private const int KeySize = 32; // Size of the hash in bytes (256 bits)
        private const int Iterations = 10000; // Number of iterations for PBKDF2

        /// <summary>
        /// Hashes the given password using PBKDF2 and returns the hash and salt as Base64-encoded strings.
        /// </summary>
        /// <param name="password">The plain-text password to hash.</param>
        /// <returns>A tuple containing the Base64-encoded hash and salt strings.</returns>
        public static (string hash, string salt) HashPassword(string password)
        {
            // Generate a new salt
            byte[] salt = GenerateSalt();

            // Derive the hash using PBKDF2
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] hash = deriveBytes.GetBytes(KeySize);

                // Convert the hash and salt to Base64 strings for storage
                string hashString = Convert.ToBase64String(hash);
                string saltString = Convert.ToBase64String(salt);

                return (hashString, saltString);
            }
        }

        /// <summary>
        /// Verifies a plain-text password against a stored Base64-encoded hash and salt strings.
        /// </summary>
        /// <param name="password">The plain-text password to verify.</param>
        /// <param name="storedHash">The Base64-encoded stored hash.</param>
        /// <param name="storedSalt">The Base64-encoded stored salt.</param>
        /// <returns>True if the password matches the hash; otherwise, false.</returns>
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            // Convert the Base64-encoded hash and salt back to byte arrays
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            // Derive the hash using the provided password and salt
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, Iterations))
            {
                byte[] computedHash = deriveBytes.GetBytes(KeySize);

                // Compare the computed hash with the stored hash
                return AreHashesEqual(computedHash, hashBytes);
            }
        }

        /// <summary>
        /// Generates a new salt.
        /// </summary>
        /// <returns>A byte array containing the generated salt.</returns>
        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        /// <summary>
        /// Compares two hash arrays in constant time.
        /// </summary>
        /// <param name="a">The first hash array.</param>
        /// <param name="b">The second hash array.</param>
        /// <returns>True if the hashes are equal; otherwise, false.</returns>
        private static bool AreHashesEqual(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            int result = 0;
            for (int i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }
            return result == 0;
        }
    }
}
