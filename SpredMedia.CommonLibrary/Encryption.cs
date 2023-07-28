using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SpredMedia.CommonLibrary
{
    public static class Encryption
    {
        public static string GenerateSHA256(string input)
        {
            byte[] hash;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                hash = sha256.ComputeHash(inputBytes);
            }

            // Convert the hash array to hexadecimal string
            StringBuilder hexBuilder = new StringBuilder(hash.Length * 2);
            for (int i = 0; i < hash.Length; i++)
            {
                hexBuilder.Append(hash[i].ToString("x2"));
            }

            string hexString = hexBuilder.ToString();
            return hexString;
        }
        public static byte[] StringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] array = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                array[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return array;
        }
        public static string DecryptData(string Text, string key, string iv)
        {
            string result = null;
            byte[] bytes = Encoding.UTF8.GetBytes(key);
            byte[] bytes2 = Encoding.UTF8.GetBytes(iv);
            byte[] buffer = StringToByteArray(Text);
            using (AesManaged aesManaged = new AesManaged())
            {
                aesManaged.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = aesManaged.CreateDecryptor(bytes, bytes2);
                using MemoryStream stream = new MemoryStream(buffer);
                using CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
                using StreamReader streamReader = new StreamReader(stream2);
                result = streamReader.ReadToEnd();
            }

            return result;
        }
        public static string EncryptData(string plainText, string key, string iv)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(key);
            byte[] bytes2 = Encoding.UTF8.GetBytes(iv);
            byte[] ba;
            using (AesManaged aesManaged = new AesManaged())
            {
                aesManaged.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = aesManaged.CreateEncryptor(bytes, bytes2);
                using MemoryStream memoryStream = new MemoryStream();
                using CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write(plainText);
                }

                ba = memoryStream.ToArray();
            }

            return ByteArrToStrg(ba);
        }

        public static string ByteArrToStrg(byte[] ba)
        {
            StringBuilder strgBuld = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                strgBuld.AppendFormat("{0:x2}", b);
            }

            return strgBuld.ToString();
        }
    }
}
