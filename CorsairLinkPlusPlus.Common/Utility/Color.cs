#region LICENSE
/**
 * CorsairLinkPlusPlus
 * Copyright (c) 2014, Mark Dietzer & Simon Schick, All rights reserved.
 *
 * CorsairLinkPlusPlus is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 *
 * CorsairLinkPlusPlus is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with CorsairLinkPlusPlus.
 */
 #endregion

namespace CorsairLinkPlusPlus.Common.Utility
{
    public struct Color
    {
        public byte R, G, B;

        public Color(Color color)
        {
            this.R = color.R;
            this.G = color.G;
            this.B = color.B;
        }

        public Color(byte r, byte g, byte b)
        {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public override string ToString()
        {
            return R.ToString() + ", " + G.ToString() + ", " + B.ToString();
        }

        public byte[] ToArray()
        {
            return new byte[] { R, G, B };
        }
    }
}
