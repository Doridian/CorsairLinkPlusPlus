using System;
using System.Text;

namespace CorsairLinkPlusPlus.Driver
{
    public static class BitCodec
    {
        public static double ToFloat(ushort input_val)
        {
            int _exponent = input_val >> 11;
            int _fraction = (int)(input_val & 2047);
            if (_exponent > 15)
                _exponent = -(32 - _exponent);

            if (_fraction > 1023)
                _fraction = -(2048 - _fraction);

            if ((_fraction & 1) == 1)
                _fraction++;

            double _rawNumber = (double)_fraction * Math.Pow(2.0, (double)_exponent);

            return (double)((int)(_rawNumber * 10.0 + 0.5)) / 10.0;
        }

        public static double ToFloat(byte[] data, int start_index = 0)
        {
            if (data == null)
                return 0.0;
            return ToFloat(BitConverter.ToUInt16(data, start_index));
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
