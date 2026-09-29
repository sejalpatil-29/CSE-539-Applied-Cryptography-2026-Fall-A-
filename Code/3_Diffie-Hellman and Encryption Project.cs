using System;
using System.Text;
using System.Numerics;
using System.Security.Cryptography;



    class DiffieHellman
    {


       public static void Main(string[] args)
        {
           //Write your code here and do not change the class name.
            byte[] iv = ParseHexBytes(args[0]);
            int N_e = int.Parse(args[3]);
            BigInteger N_c = BigInteger.Parse(args[4]);
            BigInteger N = BigInteger.Pow(2, N_e) - N_c;
            BigInteger x = BigInteger.Parse(args[5]);
            BigInteger gy = BigInteger.Parse(args[6]);
            byte[] cipherBytes = ParseHexBytes(args[7]);
            string plaintext = args[8];
        
            BigInteger keyInt = BigInteger.ModPow(gy, x, N);        
            byte[] key = ToFixedLength(keyInt.ToByteArray(), 32);
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;            
                string decryptedText;
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                    decryptedText = Encoding.UTF8.GetString(decryptedBytes);
                }            
                byte[] encryptedBytes;
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
                    encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                }
                string encryptedHex = BitConverter.ToString(encryptedBytes).Replace("-", " ");
                Console.WriteLine(decryptedText + "," + encryptedHex);
            }
        }
        static byte[] ParseHexBytes(string hexString)
        {
            string[] parts = hexString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            byte[] result = new byte[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                result[i] = Convert.ToByte(parts[i], 16);
            }
        return result;
        }
        static byte[] ToFixedLength(byte[] littleEndianBytes, int length)
        {
            byte[] result = new byte[length];
            int copyLength = Math.Min(littleEndianBytes.Length, length);
            Array.Copy(littleEndianBytes, result, copyLength);
            return result;
        }
    }

