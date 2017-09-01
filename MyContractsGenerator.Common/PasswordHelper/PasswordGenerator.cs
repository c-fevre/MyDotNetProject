using System;
using System.Linq;

namespace MyContractsGenerator.Common.PasswordHelper
{
    /// <summary>
    ///     Generate a Password
    /// </summary>
    public class PasswordGenerator
    {
        /// <summary>
        ///     Creates a pseudo-random password containing the number of character classes
        ///     defined by complexity, where 2 = alpha, 3 = alpha+num, 4 = alpha+num+special.
        /// </summary>
        public static string GeneratePassword(int length, int complexity)
        {
            System.Security.Cryptography.RNGCryptoServiceProvider csp =
                new System.Security.Cryptography.RNGCryptoServiceProvider();

            // Define the possible character classes where complexity defines the number
            // of classes to include in the final output.
            char[][] classes =
            {
                @"abcdefghijklmnopqrstuvwxyz".ToCharArray(),
                @"ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(),
                @"0123456789".ToCharArray(),
                @"*?!.".ToCharArray()
            };

            complexity = Math.Max(1, Math.Min(classes.Length, complexity));
            if (length < complexity)
            {
                throw new ArgumentOutOfRangeException("Password Generator Lenght Error");
            }

            // Since we are taking a random number 0-255 and modulo that by the number of
            // characters, characters that appear earilier in this array will recieve a
            // heavier weight. To counter this we will then reorder the array randomly.
            // This should prevent any specific character class from recieving a priority
            // based on it's order.
            char[] allchars = classes.Take(complexity).SelectMany(c => c).ToArray();
            byte[] bytes = new byte[allchars.Length];
            csp.GetBytes(bytes);
            for (int i = 0; i < allchars.Length; i++)
            {
                char tmp = allchars[i];
                allchars[i] = allchars[bytes[i] % allchars.Length];
                allchars[bytes[i] % allchars.Length] = tmp;
            }

            // Create the random values to select the characters
            Array.Resize(ref bytes, length);
            char[] result = new char[length];

            while (true)
            {
                csp.GetBytes(bytes);

                // Obtain the character of the class for each random byte
                for (int i = 0; i < length; i++)
                {
                    result[i] = allchars[bytes[i] % allchars.Length];
                }

                // Verify that it does not start or end with whitespace
                if (char.IsWhiteSpace(result[0]) || char.IsWhiteSpace(result[(length - 1) % length]))
                {
                    continue;
                }

                string testResult = new string(result);

                // Verify that all character classes are represented
                if (0 != classes.Take(complexity).Count(c => testResult.IndexOfAny(c) < 0))
                {
                    continue;
                }

                csp.Dispose();
                return testResult;
            }
        }
    }
}