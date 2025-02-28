using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Int20h2025.Auth.Utilities
{
    public static class TokenEncryptionUtility
    {
        private const int KeySize = 32;

        public static string EncryptToken(string dataToEncode, string secretKey)
        {
            var key = GetValidKey(secretKey);

            using var aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.GenerateIV();
            var iv = aesAlg.IV;

            using var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, iv);
            var tokenBytes = Encoding.UTF8.GetBytes(dataToEncode);
            var encryptedToken = encryptor.TransformFinalBlock(tokenBytes, 0, tokenBytes.Length);

            return Convert.ToBase64String(iv.Concat(encryptedToken).ToArray());
        }

        public static bool TryDecryptToken(string dataToDecode, string secretKey, out string? decryptedToken)
        {
            try
            {
                var tokenBytes = Convert.FromBase64String(dataToDecode);
                var iv = tokenBytes.Take(16).ToArray();
                var encryptedData = tokenBytes.Skip(16).ToArray();

                var key = GetValidKey(secretKey);
                using var aesAlg = Aes.Create();
                aesAlg.Key = key;
                aesAlg.IV = iv;

                using var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                var decryptedBytes = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                decryptedToken = Encoding.UTF8.GetString(decryptedBytes);
                return true;
            }
            catch
            {
                decryptedToken = null;
                return false;
            }
        }

        private static byte[] GetValidKey(string secretKey)
        {
            var keyBytes = Encoding.ASCII.GetBytes(secretKey);
            if (keyBytes.Length < KeySize)
            {
                Array.Resize(ref keyBytes, KeySize);
            }
            else if (keyBytes.Length > KeySize)
            {
                Array.Resize(ref keyBytes, KeySize);
            }

            return keyBytes;
        }
    }
}