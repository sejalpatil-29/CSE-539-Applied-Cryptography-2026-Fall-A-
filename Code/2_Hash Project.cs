using System;
using System.Collections.Generic;
using System.Security.Cryptography;



    class Hash
    {
       static Random rand = new Random();

       public static void Main(string[] args)
        {
    
           //Write your code here and do not change the class name.
            byte salt = Convert.ToByte(args[0], 16);

            using (MD5 md5 = MD5.Create())
            {
                Dictionary<long, string> seen = new Dictionary<long, string>();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                const int length = 8;

                byte[] buffer = new byte[length + 1];
                buffer[length] = salt;
                char[] strBuf = new char[length];

                while (true)
                {
                    for (int i = 0; i < length; i++)
                    {
                        int idx = rand.Next(chars.Length);
                        strBuf[i] = chars[idx];
                        buffer[i] = (byte)chars[idx];
                    }

                    byte[] hash = md5.ComputeHash(buffer);

                    long prefix = ((long)hash[0] << 32) | ((long)hash[1] << 24) | ((long)hash[2] << 16) | ((long)hash[3] << 8) | hash[4];

                    string candidate = new string(strBuf);

                    if (seen.TryGetValue(prefix, out string existing))
                    {
                        if (existing != candidate)
                        {
                            Console.WriteLine(existing + "," + candidate);
                            return;
                        }
                    }
                    else
                    {
                        seen[prefix] = candidate;
                    }
                }
            }
        }
    }

