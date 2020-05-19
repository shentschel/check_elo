using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace check_elo.Utils
{
    public static class Helper
    {
        private const string InitialVector = "pemgail9uzpgzl88";
        private const string StrPassword = "geheim";
        private const string Salt = "50Z9ybrJx965UgcE";
        
        // ReSharper disable once UnusedMember.Global
        public static string EncryptPassword(string plainPassword)
        {
            var vectorBytes = Encoding.UTF8.GetBytes(InitialVector);
            var passwordBytes = Encoding.UTF8.GetBytes(plainPassword);
            var saltBytes = Encoding.UTF8.GetBytes(Salt);
            var rfc2898Bytes = new Rfc2898DeriveBytes(StrPassword, saltBytes).GetBytes(32);
            var rijndaelManaged = new RijndaelManaged {Mode = CipherMode.CBC};
            var encryptor = rijndaelManaged.CreateEncryptor(rfc2898Bytes, vectorBytes);
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(passwordBytes, 0, passwordBytes.Length);
            cryptoStream.FlushFinalBlock();
            var encryptedPasswordAsBytes = memoryStream.ToArray();
            cryptoStream.Close();
            memoryStream.Close();

            return Convert.ToBase64String(encryptedPasswordAsBytes);
        }

        // TODO: Replace PasswordDeriveBytes with Rfc2898DeriveBytes 
        public static string DecryptPassword(string cipher)
        {
            var vectorBytes = Encoding.UTF8.GetBytes(InitialVector);
            var buffer = Convert.FromBase64String(cipher);
            var saltBytes = Encoding.UTF8.GetBytes(Salt);
            var rfc2898Bytes = new Rfc2898DeriveBytes(StrPassword, saltBytes).GetBytes(32);
            var rijndaelManaged = new RijndaelManaged {Mode = CipherMode.CBC};
            var decryptor = rijndaelManaged.CreateDecryptor(rfc2898Bytes, vectorBytes);
            var memoryStream = new MemoryStream(buffer);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var numArray = new byte[buffer.Length];
            var count = cryptoStream.Read(numArray, 0, numArray.Length);
            cryptoStream.Close();
            memoryStream.Close();

            return Encoding.UTF8.GetString(numArray, 0, count);
        }

        public static string IfNullOrWhitespaceUse(string nullableString, string replacementString)
        {
            return string.IsNullOrWhiteSpace(nullableString) ? replacementString : nullableString;
        }

        public static string GenerateDefaultIndexServerUrl(string host, string port, string archiveName)
        {
            return $"http://{host}:{port}/ix-{archiveName}/ix";
        }
    }
}