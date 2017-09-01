using System;
using System.Security.Cryptography;

namespace MyContractsGenerator.Common.PasswordHelper
{
    public static class ShaHashPassword
    {
        /// <summary>
        ///     GetSha256ResultString
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string GetSha256ResultString(string password)
        {
            string hex = string.Empty;
            using (HashAlgorithm hash = new SHA256Managed())
            {
                byte[] plainTextBytes = GetBytes(password);
                byte[] hashBytes = hash.ComputeHash(plainTextBytes);

                hex = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            }
            return hex;
        }

        /// <summary>
        ///     utilty function to convert string to byte[]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}