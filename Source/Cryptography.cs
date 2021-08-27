using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace KeyPresser
{
    public static class Cryptography
    {
        public static byte[] sha1(byte[] input)
        {
            SHA1 sha1 = SHA1.Create();
            return sha1.ComputeHash(input);
        }
        public static byte[] sha1(string input)
        {
            SHA1 sha1 = SHA1.Create();
            return sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
        }
        public static string ssha1(byte[] input)
        {
            SHA1 sha1 = SHA1.Create();
            return BitConverter.ToString(sha1.ComputeHash(input)).Replace("-", "").ToLowerInvariant();
        }
        public static string ssha1(string input)
        {
            SHA1 sha1 = SHA1.Create();
            return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "").ToLowerInvariant();
        }
    }
}
