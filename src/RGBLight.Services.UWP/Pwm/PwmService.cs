using Microsoft.IoT.Lightning.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices;
using Windows.Devices.Pwm;

namespace RGBLight.Services.Pwm
{
    public class PwmService : IPwmService
    {
        private PwmController _controller;

        internal List<PwmPin> _pins = new List<PwmPin>();

        public PwmService()
        {


        }

        public IPwmPin OpenPin(int pinNumber)
        {
            PwmPin pin = _pins.FirstOrDefault(p => p.PinNumber == pinNumber);
            if (pin == null)
            {
                var pin0 = _controller.OpenPin(pinNumber);
                pin = new PwmPin(this, pin0, pinNumber);
                _pins.Add(pin);
            }
            return pin;
        }

        public double ActualFrequency
        {
            get
            {
                return _controller.ActualFrequency;
            }
        }

        public double MaxFrequency
        {
            get
            {
                return _controller.MaxFrequency;
            }
        }

        public double MinFrequency
        {
            get
            {
                return _controller.MinFrequency;
            }
        }

        public int PinCount
        {
            get
            {
                return _controller.PinCount;
            }
        }

        public async Task InitializeAsync()
        {
            if (_controller != null) throw new InvalidOperationException("Service has already been initialize.");

            if (!Lightning.IsLightningEnabled) throw new NotSupportedException("Lightning is not supported by this device.");

            LowLevelDevicesController.DefaultProvider = LightningProvider.GetAggregateProvider();

            _controller = (await PwmController.GetControllersAsync(LightningPwmProvider.GetPwmProvider()))[1];
            _controller.SetDesiredFrequency(50);
        }

        public double SetDesiredFrequency(double desiredFrequency)
        {
            return _controller.SetDesiredFrequency(desiredFrequency);
        }
    }

    public class PwmPin : IPwmPin
    {
        private int _number;
        private Windows.Devices.Pwm.PwmPin _pin;
        private PwmService _service;

        public PwmPin(PwmService service, Windows.Devices.Pwm.PwmPin pin, int number)
        {
            _service = service;
            _pin = pin;
            _number = number;
        }

        public bool IsStarted
        {
            get
            {
                return _pin.IsStarted;
            }
        }

        public int PinNumber
        {
            get
            {
                return _number;
            }
        }

        public PwmPolarity Polarity
        {
            get
            {
                switch(_pin.Polarity)
                {
                    case PwmPulsePolarity.ActiveHigh:
                        return PwmPolarity.ActiveHigh;

                    case PwmPulsePolarity.ActiveLow:
                        return PwmPolarity.ActiveLow;
                }

                throw new Exception();
            }

            set
            {
                switch(value)
                {
                    case PwmPolarity.ActiveLow:
                        _pin.Polarity =  PwmPulsePolarity.ActiveLow;
                        break;

                    case PwmPolarity.ActiveHigh:
                        _pin.Polarity =  PwmPulsePolarity.ActiveHigh;
                        break;
                }

                throw new Exception();
            }
        }

        public void Dispose()
        {
            //_pin.Dispose();
            _service._pins.Remove(this);
        }

        public double GetActiveDutyCyclePercentage()
        {
            return _pin.GetActiveDutyCyclePercentage();
        }

        public void SetActiveDutyCyclePercentage(double dutyCyclePercentage)
        {
            _pin.SetActiveDutyCyclePercentage(dutyCyclePercentage);
        }

        public void Start()
        {
            _pin.Start();
        }

        public void Stop()
        {
            _pin.Stop();
        }
    }
}
