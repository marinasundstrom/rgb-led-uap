using Microsoft.IoT.Lightning.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGBLight.Services
{
    public static class Lightning
    {
        public static bool IsLightningEnabled
        {
            get
            {
                return LightningProvider.IsLightningEnabled;
            }
        }
    }
}
