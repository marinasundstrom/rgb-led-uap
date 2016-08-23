using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RGBLight.Services.Pwm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RGBLight.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        internal const int RED_PIN = 5;
        internal const int GREEN_PIN = 6;
        internal const int BLUE_PIN = 13;

        internal static readonly string ButtonOnText = "Stop animation";
        internal static readonly string ButtonOffText = "Start animation";

        public MainViewModel(IPwmService pwmService)
        {
            this.PwmService = pwmService;

            red = 0;
            green = 0;
            blue = 0;

            IsRunning = false;
        }

        public IPwmService PwmService { get; private set; }

        public async Task InitializeAsync()
        {
            await PwmService.InitializeAsync();

            RedPin = PwmService.OpenPin(RED_PIN);
            GreenPin = PwmService.OpenPin(GREEN_PIN);
            BluePin = PwmService.OpenPin(BLUE_PIN);

            RedPin.Start();
            GreenPin.Start();
            BluePin.Start();

            RedPin.SetActiveDutyCyclePercentage(red);
            GreenPin.SetActiveDutyCyclePercentage(green);
            BluePin.SetActiveDutyCyclePercentage(blue);

            PropertyChanged += MainViewModel_PropertyChanged;
        }

        private void MainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case RedPropertyName:
                    RedPin.SetActiveDutyCyclePercentage(PrepareValue(red));
                    break;

                case GreenPropertyName:
                    GreenPin.SetActiveDutyCyclePercentage(PrepareValue(green));
                    break;

                case BluePropertyName:
                    BluePin.SetActiveDutyCyclePercentage(PrepareValue(blue));
                    break;
            }
        }

        ~MainViewModel()
        {
            PropertyChanged -= MainViewModel_PropertyChanged;

            RedPin.Stop();
            GreenPin.Stop();
            BluePin.Stop();
        }

        /// <summary>
        /// The <see cref="Red" /> property's name.
        /// </summary>
        public const string RedPropertyName = "Red";

        private double red = 0;

        /// <summary>
        /// Sets and gets the Red property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double Red
        {
            get
            {
                return red;
            }
            set
            {
                Set(RedPropertyName, ref red, value);
            }
        }

        /// <summary>
        /// The <see cref="Green" /> property's name.
        /// </summary>
        public const string GreenPropertyName = "Green";

        private double green = 0;

        /// <summary>
        /// Sets and gets the Green property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double Green
        {
            get
            {
                return green;
            }
            set
            {
                Set(GreenPropertyName, ref green, value);
            }
        }

        /// <summary>
        /// The <see cref="Blue" /> property's name.
        /// </summary>
        public const string BluePropertyName = "Blue";

        private double blue = 0;

        /// <summary>
        /// Sets and gets the Blue property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double Blue
        {
            get
            {
                return blue;
            }
            set
            {
                Set(BluePropertyName, ref blue, value);
            }
        }

        public IPin RedPin { get; private set; }
        public IPin GreenPin { get; private set; }
        public IPin BluePin { get; private set; }

        private RelayCommand _runCommand;
        private CancellationTokenSource cts;

        /// <summary>
        /// Gets the RunCommand.
        /// </summary>
        public RelayCommand RunCommand
        {
            get
            {
                return _runCommand
                    ?? (_runCommand = new RelayCommand(
                    () =>
                    {
                        if (IsRunning)
                        {
                            cts.Cancel();
                            IsRunning = false;
                            ButtonText = ButtonOffText;

                            cts = null;
                        }
                        else
                        {
                            cts = new CancellationTokenSource();

                            IsRunning = true;
                            Animate(cts.Token);

                            ButtonText = ButtonOnText;
                        }
                    }));
            }
        }

        /// <summary>
        /// The <see cref="Interpolate" /> property's name.
        /// </summary>
        public const string InterpolatePropertyName = "Interpolate";

        private bool interpolate = false;

        /// <summary>
        /// Sets and gets the Interpolate property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool Interpolate
        {
            get
            {
                return interpolate;
            }
            set
            {
                Set(() => Interpolate, ref interpolate, value);
            }
        }

        public async Task Animate(CancellationToken ct)
        {
            if (Interpolate)
            {
                int seconds = 0;
                int step = 0;

                Color resultColor;

                while (true)
                {
                    if (ct.IsCancellationRequested) return;

                    if (seconds == 30)
                    {
                        seconds = 0;
                    }

                    resultColor = Animation.InterpolateColors(seconds);

                    SetColor(resultColor);

                    await Task.Delay(1000);

                    seconds++;
                }
            }
            else
            {
                while (true)
                {
                    if (ct.IsCancellationRequested) return;

                    SetColor(Color.Red);
                    await Task.Delay(1000);

                    if (ct.IsCancellationRequested) return;

                    SetColor(Color.Green);
                    await Task.Delay(1000);

                    if (ct.IsCancellationRequested) return;

                    SetColor(Color.Blue);
                    await Task.Delay(1000);
                }
            }
        }

  

        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
            set
            {
                Set(ref isRunning, value);

                IsNotRunning = !IsRunning;
            }
        }

        public bool IsNotRunning
        {
            get
            {
                return isNotRunning;
            }
            private set
            {
                Set(ref isNotRunning, value);
            }
        }

        /// <summary>
        /// The <see cref="ButtonText" /> property's name.
        /// </summary>
        public const string ButtonTextPropertyName = "ButtonText";

        private string _buttonText = ButtonOffText;
        private bool isRunning;
        private bool isNotRunning;

        /// <summary>
        /// Sets and gets the ButtonText property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string ButtonText
        {
            get
            {
                return _buttonText;
            }

            set
            {
                if (_buttonText == value)
                {
                    return;
                }

                var oldValue = _buttonText;
                _buttonText = value;
                RaisePropertyChanged(ButtonTextPropertyName, oldValue, value, true);
            }
        }

        private double PrepareValue(double v)
        {
#if COMMON_ANODE
            return 1 - v;
#else
            return v;
#endif
        }

        private void SetColor(double r, double g, double b)
        {
            Red = r;
            Green = g;
            Blue = b;
        }

        private void SetColorBytes(byte r, byte g, byte b)
        {
            var r_ = ((double)1 / 255) * r;
            var g_ = ((double)1 / 255) * g;
            var b_ = ((double)1 / 255) * b;
            SetColor(r_, g_, b_);
        }

        private void SetColor(Color color)
        {
            SetColorBytes(color.R, color.G, color.B);
        }
    }
}
