using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBLight.Services.Pwm
{
    public class DummyPwmService : IPwmService
    {
        internal List<IPin> _pins = new List<IPin>();
        private double _desiredFrequency;

        public DummyPwmService()
        {
            ActualFrequency = 0;
            MinFrequency = 0;
            MaxFrequency = 50;
        }

        public double ActualFrequency
        {
            get; internal set;
        }

        public double MaxFrequency
        {
            get; internal set;
        }

        public double MinFrequency
        {
            get; internal set;
        }

        public int PinCount
        {
            get
            {
                return _pins.Count;
            }
        }

        public async Task InitializeAsync()
        {
            
        }

        public IPin OpenPin(int pinNumber)
        {
            IPin pin = _pins.FirstOrDefault(p => p.PinNumber == pinNumber);
            if (pin == null)
            {
                pin = new DummyPin(this, pinNumber);
                _pins.Add(pin);
            }
            return pin;
        }

        public double SetDesiredFrequency(double desiredFrequency)
        {
            ActualFrequency =  (_desiredFrequency = desiredFrequency);

            return ActualFrequency;
        }
    }

    public class DummyPin : IPin
    {
        private double _activeDutyCyclePercentage;
        private DummyPwmService _service;
        private bool _started;

        public DummyPin(DummyPwmService service, int pinNumber)
        {
            _service = service;
            PinNumber = pinNumber;
        }

        public bool IsStarted
        {
            get
            {
                return _started;
            }
        }

        public int PinNumber
        {
            get;
        }

        public PwmPolarity Polarity
        {
            get;
            set;
        }

        public void Dispose()
        {
            _service._pins.Remove(this);
        }

        public double GetActiveDutyCyclePercentage()
        {
            return _activeDutyCyclePercentage;
        }

        public void SetActiveDutyCyclePercentage(double dutyCyclePercentage)
        {
            _activeDutyCyclePercentage = dutyCyclePercentage;
        }

        public void Start()
        {
            _started = true;
        }

        public void Stop()
        {
            _started = false;
        }
    }
}
