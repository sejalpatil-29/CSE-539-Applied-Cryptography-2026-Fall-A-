using System;
using System.IO;
using System.Collections;


    class Steganography
    {


       public static void Main(string[] args)
        {
           //Write your code here and do not change the class name.
         byte[] bmpHeader = new byte[] {
            0x42,0x4D,0x4C,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x1A,0x00,0x00,0x00,0x0C,0x00,
            0x00,0x00,0x04,0x00,0x04,0x00,0x01,0x00,
            0x18,0x00
        };
        byte[] colorBytes = new byte[] {
            0x00,0x00,0xFF,0xFF,0xFF,0xFF,
            0x00,0x00,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
            0xFF,0x00,0x00,0x00,0xFF,0xFF,0xFF,0x00,
            0x00,0x00,0xFF,0x00,0x00,0xFF,0xFF,0xFF,
            0xFF,0x00,0x00,0xFF,0xFF,0xFF,0xFF,0xFF,
            0xFF,0x00,0x00,0x00,0xFF,0xFF,0xFF,0x00,
            0x00,0x00
        };

        string[] hexParts = args[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        byte[] hiddenData = new byte[hexParts.Length];
        for (int i = 0; i < hexParts.Length; i++)
        {
            hiddenData[i] = Convert.ToByte(hexParts[i], 16);
        }

        int[] twoBitGroups = new int[hiddenData.Length * 4];
        int idx = 0;
        foreach (byte b in hiddenData)
        {
            twoBitGroups[idx++] = (b >> 6) & 0x03;
            twoBitGroups[idx++] = (b >> 4) & 0x03;
            twoBitGroups[idx++] = (b >> 2) & 0x03;
            twoBitGroups[idx++] = b & 0x03;
        }

        byte[] modifiedColors = (byte[])colorBytes.Clone();
        for (int i = 0; i < modifiedColors.Length && i < twoBitGroups.Length; i++)
        {
            int lowBits = modifiedColors[i] & 0x03;
            int newLowBits = lowBits ^ twoBitGroups[i];
            modifiedColors[i] = (byte)((modifiedColors[i] & 0xFC) | newLowBits);
        }

        byte[] result = new byte[bmpHeader.Length + modifiedColors.Length];
        Array.Copy(bmpHeader, 0, result, 0, bmpHeader.Length);
        Array.Copy(modifiedColors, 0, result, bmpHeader.Length, modifiedColors.Length);

        Console.WriteLine(BitConverter.ToString(result).Replace("-", " "));
        }
    }

