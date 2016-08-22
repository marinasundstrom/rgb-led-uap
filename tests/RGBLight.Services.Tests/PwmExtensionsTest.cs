using RGBLight.Services.Pwm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RGBLight.Services.Tests
{
    public class PwmExtensionsTest
    {
        [Fact]
        public void CalculateValues()
        {
            var result = PwmExtensions.CalculateByteValueFromPercentage(0);

            Assert.True(result == 0);

            result = PwmExtensions.CalculateByteValueFromPercentage(1);

            Assert.True(result == 255);

            result = PwmExtensions.CalculateByteValueFromPercentage(0.5);

            Assert.True(result == 128);

            var result1 = PwmExtensions.CalculatePercentageFromByteValue(134);
            var result2 = PwmExtensions.CalculateByteValueFromPercentage(result1);

            Assert.True(result2 == 134);
        }
    }
}
