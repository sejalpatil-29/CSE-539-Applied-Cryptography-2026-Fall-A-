using System;
using System.Numerics;

    class RSA
    {
   public static void Main(string[] args)
    {
    //Write your code here and do not change the class name.
        int p_e = int.Parse(args[0]);
        BigInteger p_c = BigInteger.Parse(args[1]);
        int q_e = int.Parse(args[2]);
        BigInteger q_c = BigInteger.Parse(args[3]);
        BigInteger ciphertext = BigInteger.Parse(args[4]);
        BigInteger plaintext = BigInteger.Parse(args[5]);
        BigInteger p = BigInteger.Pow(2, p_e) - p_c;
        BigInteger q = BigInteger.Pow(2, q_e) - q_c;
        BigInteger n = p * q;
        BigInteger phi = (p - 1) * (q - 1);
        BigInteger e = 65537;
        BigInteger d = ModInverse(e, phi);

        BigInteger decrypted = BigInteger.ModPow(ciphertext, d, n);
        BigInteger encrypted = BigInteger.ModPow(plaintext, e, n);
        Console.WriteLine(decrypted + "," + encrypted);
    }
        static BigInteger ModInverse(BigInteger a, BigInteger m)
    {
        BigInteger oldR = a, r = m;
        BigInteger oldS = 1, s = 0;

        while (r != 0)
        {
            BigInteger quotient = oldR / r;

            BigInteger tempR = r;
            r = oldR - quotient * r;
            oldR = tempR;

            BigInteger tempS = s;
            s = oldS - quotient * s;
            oldS = tempS;
        }
        BigInteger result = oldS % m;
        if (result < 0)
        {
            result += m;
        }
        return result;
    }

    }

