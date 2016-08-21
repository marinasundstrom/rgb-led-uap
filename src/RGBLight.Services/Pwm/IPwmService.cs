using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBLight.Services.Pwm
{
    public interface IPwmService
    {
        Task InitializeAsync();

        IPin OpenPin(int pinNumber);

        int PinCount { get; }

        double SetDesiredFrequency(double desiredFrequency);

        double MinFrequency { get; }

        double MaxFrequency { get; }

        double ActualFrequency { get; }
    }

    public interface IPin : IDisposable
    {
        int PinNumber { get; }

        void Start();
        void Stop();

        bool IsStarted { get; }

        PwmPolarity Polarity { get; set; }

        double GetActiveDutyCyclePercentage();
        void SetActiveDutyCyclePercentage(double dutyCyclePercentage);
    }

    public enum PwmPolarity
    {
        ActiveLow,
        ActiveHigh
    }
}
