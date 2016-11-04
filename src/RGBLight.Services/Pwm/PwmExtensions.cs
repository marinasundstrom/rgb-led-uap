using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBLight.Services.Pwm
{
    public static class PwmExtensions
    {
        private static double byteRatio;
        private static double percentRatio;

        static PwmExtensions()
        {
            percentRatio = Byte.MaxValue / (double)100;
            byteRatio = (double)1 / Byte.MaxValue;
        }

        public static byte GetActiveDutyCycleByte(this IPwmPin pin)
        {
            var dutyCyclePercentage = pin.GetActiveDutyCyclePercentage();
            return CalculateByteValueFromPercentage(dutyCyclePercentage);
        }

        public static void SetActiveDutyCycleByte(this IPwmPin pin, byte dutyCycle)
        {
            var value = CalculatePercentageFromByteValue(dutyCycle);
            pin.SetActiveDutyCyclePercentage(value);
        }

         internal static byte CalculateByteValueFromPercentage(double p)
        {
            return (byte)Math.Ceiling(percentRatio * (p * 100));
        }

        internal static double CalculatePercentageFromByteValue(byte b)
        {
            return byteRatio * b;
        }
    }
}
