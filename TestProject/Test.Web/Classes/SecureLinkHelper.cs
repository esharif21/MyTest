using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Isp.Web.Classes
{
    public static class SecureLinkHelper
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("MySuperStrongKey1234567890123456"); // 32 bytes
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("StrongInitVector"); // 16 bytes

        public static string GenerateVerificationLink(string userId, string baseUrl, int expiryMinutes = 30)
        {
            var expiry = DateTimeOffset.UtcNow.AddMinutes(expiryMinutes).ToUnixTimeSeconds();
            var payload = $"{userId}|{expiry}";

            var encrypted = Encrypt(payload);
            var urlSafe = Base64UrlEncode(encrypted);

            return $"{baseUrl}verify?token={urlSafe}";
        }

        public static string GeneratePasswordResetLink(string userId, string baseUrl, int expiryMinutes = 30)
        {
            var expiry = DateTimeOffset.UtcNow.AddMinutes(expiryMinutes).ToUnixTimeSeconds();
            var payload = $"{userId}|{expiry}";

            var encrypted = Encrypt(payload);
            var urlSafe = Base64UrlEncode(encrypted);

            return $"{baseUrl}resetpassword?token={urlSafe}";
        }

        public static bool ValidateToken(string token, out string userId)
        {
            userId = null;
            try
            {
                var base64 = Base64UrlDecode(token);
                var decrypted = Decrypt(base64);

                var parts = decrypted.Split('|');
                if (parts.Length != 2) return false;

                userId = parts[0];
                var expiry = long.Parse(parts[1]);

                var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                return now <= expiry;
            }
            catch
            {
                return false;
            }
        }

        private static string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        private static string Decrypt(string cipherText)
        {
            var buffer = Convert.FromBase64String(cipherText);
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var ms = new MemoryStream(buffer);
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }

        // URL-safe Base64 (replaces +,/ with -,_, removes =)
        private static string Base64UrlEncode(string base64)
        {
            return base64.Replace("+", "-").Replace("/", "_").TrimEnd('=');
        }

        private static string Base64UrlDecode(string urlSafe)
        {
            string base64 = urlSafe.Replace("-", "+").Replace("_", "/");
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return base64;
        }
    }
}
