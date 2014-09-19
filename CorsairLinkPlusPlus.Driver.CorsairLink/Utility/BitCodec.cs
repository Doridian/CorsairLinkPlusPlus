using System;
using System.Text;

namespace CorsairLinkPlusPlus.Driver.CorsairLink
{
    public static class BitCodec
    {
        private static double ToFloat(ushort inputValue)
        {
            int exponent = inputValue >> 11;
            int fraction = (int)(inputValue & 2047);
            if (exponent > 15)
                exponent = -(32 - exponent);

            if (fraction > 1023)
                fraction = -(2048 - fraction);

            if ((fraction & 1) == 1)
                fraction++;

            double rawNumber = (double)fraction * Math.Pow(2.0, (double)exponent);

            return (double)((int)(rawNumber * 10.0 + 0.5)) / 10.0;
        }

        public static double ToFloat(byte[] data, int startIndex = 0)
        {
            if (data == null)
                return 0.0;
            return ToFloat(BitConverter.ToUInt16(data, startIndex));
        }

        public static byte[] FromFloat(double value, int exponent)
        {
            byte[] array = new byte[2];
            int num;
            if (value >= 0.0)
                num = Math.Min((int)Math.Round(value * Math.Pow(2.0, (double)exponent)), 1023);
            else
            {
                num = Math.Max((int)Math.Round(value * Math.Pow(2.0, (double)exponent)), -1023);
                num &= 2047;
            }
            array[0] = (byte)(num & 255);
            if (exponent > 0)
                exponent = 256 - exponent;

            else
                exponent = -exponent;
            
            exponent = (exponent << 3 & 255);
            array[1] = (byte)((num >> 8 & 255) | exponent);
            return array;
        }
    }
}
