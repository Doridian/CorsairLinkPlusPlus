using System;
using System.Text;

namespace CorsairLinkPlusPlus.Driver
{
    public static class CorsairBitCodec
    {
        public static double ToFloat(ushort input_val)
        {
            int num = input_val >> 11;
            int num2 = (int)(input_val & 2047);
            if (num > 15)
            {
                num = -(32 - num);
            }
            if (num2 > 1023)
            {
                num2 = -(2048 - num2);
            }
            if ((num2 & 1) == 1)
            {
                num2++;
            }
            double num3 = (double)num2 * Math.Pow(2.0, (double)num);
            return (double)((int)(num3 * 10.0 + 0.5)) / 10.0;
        }
        public static double ToFloat(byte[] data, int start_index = 0)
        {
            if (data == null)
            {
                return 0.0;
            }
            return ToFloat(BitConverter.ToUInt16(data, start_index));
        }
        public static byte[] FromFloat(double value, int exponent)
        {
            byte[] array = new byte[2];
            int num;
            if (value >= 0.0)
            {
                num = (int)Math.Round(value * Math.Pow(2.0, (double)exponent));
                if (num > 1023)
                {
                    num = 1023;
                }
            }
            else
            {
                num = (int)Math.Round(value * Math.Pow(2.0, (double)exponent));
                if (num < -1023)
                {
                    num = -1023;
                }
                num &= 2047;
            }
            array[0] = (byte)(num & 255);
            if (exponent > 0)
            {
                exponent = 256 - exponent;
            }
            else
            {
                exponent = -exponent;
            }
            exponent = (exponent << 3 & 255);
            array[1] = (byte)((num >> 8 & 255) | exponent);
            return array;
        }
    }
}
