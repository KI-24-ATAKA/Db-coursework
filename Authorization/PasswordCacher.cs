using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Authorization
{
    internal class PasswordCacher
    {
        private readonly string EncryptionKey;
        private static readonly byte[] Salt = Encoding.UTF8.GetBytes("SimpleSalt");
        public string CachePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (var aes = Aes.Create())
            {
                var key = new Rfc2898DeriveBytes(EncryptionKey, Salt, 1000);
                aes.Key = key.GetBytes(32);
                aes.IV = key.GetBytes(16);

                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(password);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
        public string UncachePassword(string encryptedPassword)
        {
            if (string.IsNullOrEmpty(encryptedPassword))
            {
                return string.Empty;
            }
            try
            {
                using (var aes = Aes.Create())
                {
                    var key = new Rfc2898DeriveBytes(EncryptionKey, Salt, 1000);
                    aes.Key = key.GetBytes(32);
                    aes.IV = key.GetBytes(16);

                    var cipherBytes = Convert.FromBase64String(encryptedPassword);

                    using (var decryptor = aes.CreateDecryptor())
                    using (var ms = new MemoryStream(cipherBytes))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        public PasswordCacher(string Key) 
        {
            EncryptionKey = Key;
        }
    }
}
