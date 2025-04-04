using System;
namespace CMPNatural.Core.Helper
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class PasswordGenerator
    {
        public static string GenerateSecurePassword(int length = 12)
        {
            // Define character sets
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string special = "!@#$%^&*()-_=+[]{}|;:,.<>?";

            // Create a single character set with all allowed characters
            string validChars = upper + lower + numbers + special;

            // Create byte array for cryptographic random number
            byte[] randomBytes = new byte[length];

            // Generate cryptographically secure random bytes
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }

            // Build the password
            StringBuilder password = new StringBuilder(length);
            foreach (byte b in randomBytes)
            {
                // Convert byte to character index
                password.Append(validChars[b % validChars.Length]);
            }

            // Ensure at least one character from each set
            password[0] = upper[RandomNumberGenerator.GetInt32(upper.Length)];
            password[1] = lower[RandomNumberGenerator.GetInt32(lower.Length)];
            password[2] = numbers[RandomNumberGenerator.GetInt32(numbers.Length)];
            password[3] = special[RandomNumberGenerator.GetInt32(special.Length)];

            // Shuffle the result to mix the guaranteed characters
            return Shuffle(password.ToString());
        }

        private static string Shuffle(string str)
        {
            char[] array = str.ToCharArray();
            int n = array.Length;
            while (n > 1)
            {
                byte[] box = new byte[1];
                RandomNumberGenerator.Fill(box);
                int k = box[0] % n;
                n--;
                (array[k], array[n]) = (array[n], array[k]);
            }
            return new string(array);
        }
    }
}

