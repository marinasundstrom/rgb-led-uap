using System;
using System.Collections.Generic;
using System.Text;

namespace RGBLight
{
    public struct Color
    {
        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public byte B { get; private set; }
        public byte G { get; private set; }
        public byte R { get; private set; }

        public static readonly Color Red = new Color(255, 0, 0);

        public static readonly Color Green = new Color(0, 255, 0);

        public static readonly Color Blue = new Color(0, 0, 255);

        public static Color Interpolate(Color from, Color to, int stepNumber, int lastStepNumber)
        {
            var r = Algorithms.Interpolate(from.R, to.R, stepNumber, lastStepNumber);
            var g = Algorithms.Interpolate(from.G, to.G, stepNumber, lastStepNumber);
            var b = Algorithms.Interpolate(from.B, to.B, stepNumber, lastStepNumber);

            return new Color((byte)r, (byte)g, (byte)b);
        }

        public static Color FromRGB(byte r, byte g, byte b)
        {
            return new Color(r, g, b);
        }

        public static Color FromPercent(double r, double g, double b)
        {
            byte r_ = ByteValueFromPercentOfByte(r);
            byte g_ = ByteValueFromPercentOfByte(g);
            byte b_ = ByteValueFromPercentOfByte(b);

            return new Color(r_, g_, b_);
        }

        private static byte ByteValueFromPercentOfByte(double percent)
        {
            return (byte)((255 / 100) * percent);
        }
    }
}
