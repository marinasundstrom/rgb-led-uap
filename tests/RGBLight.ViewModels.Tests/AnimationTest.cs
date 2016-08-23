using RGBLight.Services.Pwm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RGBLight.ViewModels.Tests
{
    public class AnimationTest
    {
        [Fact]
        public void InterpolateColors()
        {
            int seconds;

            Color resultColor;
            Color compareColor;

            Color color0 = Color.Red;
            Color color1 = Color.Green;
            Color color2 = Color.Blue;
            Color color3 = Color.Red;

            Assert.Equal(color0, color3);


            // ArgumentOutOfRange

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                resultColor = Animation.InterpolateColors(-1);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                resultColor = Animation.InterpolateColors(31);
            });


            // State #1

            seconds = 0;

            resultColor = Animation.InterpolateColors(seconds);

            Assert.Equal(resultColor, color0);


            // Middle state #1

            seconds = 5;

            resultColor = Animation.InterpolateColors(seconds);

            compareColor = Color.Interpolate(color0, color1, 5, 10);
            Assert.Equal(resultColor, compareColor);


            // State #2

            seconds = 10;

            resultColor = Animation.InterpolateColors(seconds);

            Assert.Equal(resultColor, color1);


            // Middle state #2

            seconds = 15;

            resultColor = Animation.InterpolateColors(seconds);

            compareColor = Color.Interpolate(color1, color2, 5, 10);
            Assert.Equal(resultColor, compareColor);


            // State #3

            seconds = 20;

            resultColor = Animation.InterpolateColors(seconds);

            Assert.Equal(resultColor, color2);


            // Middle state #3

            seconds = 25;

            resultColor = Animation.InterpolateColors(seconds);

            compareColor = Color.Interpolate(color2, color3, 5, 10);
            Assert.Equal(resultColor, compareColor);


            // State #4

            seconds = 30;

            resultColor = Animation.InterpolateColors(seconds);

            Assert.Equal(resultColor, color3);
        }
    }
}
