using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using RGBLight.Services;
using RGBLight.Services.Gpio;
using RGBLight.Services.Pwm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBLight.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            var ioc = SimpleIoc.Default;

            ioc.Register<MainViewModel>();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                ioc.Register<IGpioService, DummyGpioService>();
            }
            else
            {
#if !TEST

                ioc.Register<IGpioService, GpioService>();
                ioc.Register<IPwmService, PwmService>();
#endif
            }

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
    }
}
