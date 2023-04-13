using System;
using System.Security.Cryptography;
using System.Text;

namespace ProjectManager.Shared.Helper
{
    public static class EncriptFunctionsHelper
    {
        public static string GenerateMd5String(string strChange)
        {
            var md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(Encoding.UTF8.GetBytes(strChange));
            var encodedBytes = md5.Hash;
            string encodedPassword = BitConverter.ToString(encodedBytes);
            return encodedPassword;
        }

        public static bool VerifyMd5String(string str, string hash)
        {
            var hashinput = GenerateMd5String(str);
            var comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashinput, hash);
        }

        public static string GeneratePassword(string password)
        {
            return GenerateMd5String(password);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return VerifyMd5String(password, hash);
        }
    }
}
