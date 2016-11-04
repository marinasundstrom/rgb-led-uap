﻿using Microsoft.IoT.Lightning.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices;
using Windows.Devices.Gpio;

namespace RGBLight.Services.Gpio
{
    public sealed class GpioService : IGpioService
    {
        private GpioController _controller;

        internal List<GPin> _pins = new List<GPin>();

        public int PinCount
        {
            get
            {
                return _controller.PinCount;
            }
        }

        public GpioService()
        {

        }

        public async Task InitializeAsync()
        {
            if (_controller != null) throw new InvalidOperationException("Service has already been initialize.");

            if (!Lightning.IsLightningEnabled) throw new NotSupportedException("Lightning is not supported by this device.");

            LowLevelDevicesController.DefaultProvider = LightningProvider.GetAggregateProvider();

            var x =  await GpioController.GetControllersAsync(LightningGpioProvider.GetGpioProvider());

            _controller = (await GpioController.GetControllersAsync(LightningGpioProvider.GetGpioProvider()))[0];
        }

        public IGPin OpenPin(int pinNumber)
        {
            GPin pin = _pins.FirstOrDefault(p => p.PinNumber == pinNumber);
            if (pin == null)
            {
                var pin0 = _controller.OpenPin(pinNumber);
                pin = new GPin(this, pin0);
                _pins.Add(pin);
            }
            return pin;
        }
    }

    public class GPin : IGPin
    {
        private readonly GpioPin _pin;
        private readonly GpioService _service;

        public event EventHandler<GPinEventArgs> ValueChanged;

        public GPin(GpioService service, GpioPin pin)
        {
            _service = service;
            _pin = pin;
            _pin.ValueChanged += _pin_ValueChanged;
        }

        private void _pin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            ValueChanged?.Invoke(this, new GPinEventArgs((GpioPinEdge)args.Edge));
        }

        public int PinNumber
        {
            get
            {
                return _pin.PinNumber;
            }
        }

        public TimeSpan DebounceTimeout
        {
            get { return _pin.DebounceTimeout; }
            set { _pin.DebounceTimeout = value; }
        }

        public void Write(PinValue value)
        {
            GpioPinValue newValue = GpioPinValue.High;
            switch (value)
            {
                case PinValue.High:
                    newValue = GpioPinValue.High;
                    break;
                case PinValue.Low:
                    newValue = GpioPinValue.Low;
                    break;
            }
            _pin.Write(newValue);
        }

        public PinValue Read()
        {
            var value = _pin.Read();
            switch (value)
            {
                case GpioPinValue.High:
                    return PinValue.High;

                case GpioPinValue.Low:
                    return PinValue.Low;
            }

            throw new Exception();
        }

        public PinMode Mode
        {
            get
            {
                var mode = _pin.GetDriveMode();
                switch (mode)
                {
                    case GpioPinDriveMode.Input:
                        return PinMode.Input;

                    case GpioPinDriveMode.Output:
                        return PinMode.Output;

                    case GpioPinDriveMode.InputPullDown:
                        return PinMode.InputPullDown;

                    case GpioPinDriveMode.InputPullUp:
                        return PinMode.InputPullUp;
                }

                throw new Exception();
            }
        }

        public void SetMode(PinMode mode)
        {
            GpioPinDriveMode newMode = GetGpioPinDriveMode(mode);
            _pin.SetDriveMode(newMode);
        }

        private static GpioPinDriveMode GetGpioPinDriveMode(PinMode mode)
        {
            GpioPinDriveMode newMode = GpioPinDriveMode.Output;
            switch (mode)
            {
                case PinMode.Input:
                    newMode = GpioPinDriveMode.Input;
                    break;
                case PinMode.Output:
                    newMode = GpioPinDriveMode.Output;
                    break;

                case PinMode.InputPullUp:
                    newMode = GpioPinDriveMode.InputPullUp;
                    break;
                case PinMode.InputPullDown:
                    newMode = GpioPinDriveMode.InputPullDown;
                    break;
            }

            return newMode;
        }

        public void Dispose()
        {
            //_pin.Dispose();
            _service._pins.Remove(this);
            ValueChanged = null;
        }

        public bool IsDriveModeSupported(PinMode pinMode)
        {
            var driveMode = GetGpioPinDriveMode(pinMode);
            return this._pin.IsDriveModeSupported(driveMode);
        }
    }
}
