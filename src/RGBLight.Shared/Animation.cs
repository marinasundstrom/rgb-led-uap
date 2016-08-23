using System;
using System.Collections.Generic;
using System.Text;

namespace RGBLight
{
    /// <summary>
    /// Holds the animation.
    /// </summary>
    public static class Animation
    {
        public static Color InterpolateColors(int seconds)
        {
            if (seconds < 0 || seconds > 30)
            {
                throw new ArgumentOutOfRangeException(nameof(seconds), "Must not be less than 0 or greater than 30.");
            }

            Color resultColor;
            Color from = Color.Red;
            Color to = Color.Green;

            int step = 0;

            if (seconds < 10)
            {
                step = seconds;

                from = Color.Red;
                to = Color.Green;
            }
            else if (seconds < 20)
            {
                step = seconds - 10;

                from = Color.Green;
                to = Color.Blue;
            }
            else if (seconds <= 30)
            {
                step = seconds - 20;

                from = Color.Blue;
                to = Color.Red;
            }

            resultColor = Color.Interpolate(from, to, step, 10);
            return resultColor;
        }
    }
}
