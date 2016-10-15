using RGBLight.Services.Gpio;
using RGBLight.Services.Pwm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RGBLight.ViewModels.Tests
{
    public class MainViewModelTest
    {
        [Fact]
        public async Task InitializeViewModel()
        {
            var gpioService = new DummyGpioService();
            var pwmService = new DummyPwmService();

            MainViewModel viewModel = new MainViewModel(gpioService, pwmService);

            await viewModel.InitializeAsync();

            Assert.Equal(viewModel.Red, 0);
            Assert.Equal(viewModel.Green, 0);
            Assert.Equal(viewModel.Blue, 0);
        }

        [Fact]
        public async Task Animate()
        {
            var gpioService = new DummyGpioService();
            var pwmService = new DummyPwmService();

            MainViewModel viewModel = new MainViewModel(gpioService, pwmService);

            await viewModel.InitializeAsync();

            Assert.Same(viewModel.ButtonText, MainViewModel.ButtonOffText);

            viewModel.RunCommand.Execute(null);

            Assert.Same(viewModel.ButtonText, MainViewModel.ButtonOnText);
            Assert.True(viewModel.IsRunning);
            Assert.False(viewModel.IsNotRunning);

            await Task.Delay(200);

            viewModel.RunCommand.Execute(null);

            await Task.Delay(200);

            Assert.Same(viewModel.ButtonText, MainViewModel.ButtonOffText);
            Assert.False(viewModel.IsRunning);
            Assert.True(viewModel.IsNotRunning);
        }

        [Fact]
        public async Task AnimateWithInterpolateTrue()
        {
            var gpioService = new DummyGpioService();
            var pwmService = new DummyPwmService();

            MainViewModel viewModel = new MainViewModel(gpioService, pwmService);

            await viewModel.InitializeAsync();

            Assert.Same(viewModel.ButtonText, MainViewModel.ButtonOffText);

            viewModel.Interpolate = true;
            viewModel.RunCommand.Execute(null);

            Assert.Same(viewModel.ButtonText, MainViewModel.ButtonOnText);
            Assert.True(viewModel.IsRunning);
            Assert.False(viewModel.IsNotRunning);

            await Task.Delay(200);

            viewModel.RunCommand.Execute(null);

            await Task.Delay(200);

            Assert.Same(viewModel.ButtonText, MainViewModel.ButtonOffText);
            Assert.False(viewModel.IsRunning);
            Assert.True(viewModel.IsNotRunning);
        }

        [Fact]
        public async Task SetColorRed()
        {
            var gpioService = new DummyGpioService();
            var pwmService = new DummyPwmService();

            MainViewModel viewModel = new MainViewModel(gpioService, pwmService);

            await viewModel.InitializeAsync();

            viewModel.Red = 0;

            Assert.Equal(viewModel.Red, viewModel.RedPin.GetActiveDutyCyclePercentage());

            viewModel.Red = 1;

            Assert.Equal(viewModel.Red, viewModel.RedPin.GetActiveDutyCyclePercentage());

            viewModel.Red = 0.5;

            Assert.Equal(viewModel.Red, viewModel.RedPin.GetActiveDutyCyclePercentage());
        }

        [Fact]
        public async Task SetColorGreen()
        {
            var gpioService = new DummyGpioService();
            var pwmService = new DummyPwmService();

            MainViewModel viewModel = new MainViewModel(gpioService, pwmService);

            await viewModel.InitializeAsync();

            viewModel.Green = 0;

            Assert.Equal(viewModel.Green, viewModel.GreenPin.GetActiveDutyCyclePercentage());

            viewModel.Green = 1;

            Assert.Equal(viewModel.Green, viewModel.GreenPin.GetActiveDutyCyclePercentage());

            viewModel.Green = 0.5;

            Assert.Equal(viewModel.Green, viewModel.GreenPin.GetActiveDutyCyclePercentage());
        }

        /// <summary>
        /// Note: Pin values are only valid for "common cathode" RGB LED:s.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SetColorBlue()
        {
            var gpioService = new DummyGpioService();
            var pwmService = new DummyPwmService();

            MainViewModel viewModel = new MainViewModel(gpioService, pwmService);

            await viewModel.InitializeAsync();

            viewModel.Blue = 0;

            Assert.Equal(viewModel.Blue, viewModel.BluePin.GetActiveDutyCyclePercentage());

            viewModel.Blue = 1;

            Assert.Equal(viewModel.Blue, viewModel.BluePin.GetActiveDutyCyclePercentage());

            viewModel.Blue = 0.5;

            Assert.Equal(viewModel.Blue, viewModel.BluePin.GetActiveDutyCyclePercentage());
        }
    }
}
