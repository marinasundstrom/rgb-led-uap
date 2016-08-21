using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBLight.Services.Gpio
{
    public interface IGpioService
    {
        IPin OpenPin(int pinNumber);

        int PinCount { get; }
    }

    public interface IPin : IDisposable
    {
        PinMode Mode { get; }
        int PinNumber { get; }
        TimeSpan DebounceTimeout { get; set; }
        PinValue Read();
        void SetMode(PinMode mode);
        void Write(PinValue value);

        event EventHandler<GPinEventArgs> ValueChanged;
    }

    public enum PinValue
    {
        Low,
        High
    }

    public enum PinMode
    {
        Input,
        Output,
        InputPullUp,
        InputPullDown
    }

    public class GPinEventArgs : EventArgs
    {
        private GpioPinEdge edge;

        public GPinEventArgs(GpioPinEdge edge)
        {
            this.edge = edge;
        }

        public GpioPinEdge Edge
        {
            get
            {
                return edge;
            }
        }
    }

    public enum GpioPinEdge
    {
        FallingEdge,
        RisingEdge
    }

}
