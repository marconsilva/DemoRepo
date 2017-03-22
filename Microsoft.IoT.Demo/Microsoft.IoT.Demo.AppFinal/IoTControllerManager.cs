using Microsoft.IoT.Lightning.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices;
using Windows.Devices.Gpio;
using Windows.Devices.Pwm;

namespace Microsoft.IoT.Demo.AppFinal
{
    public sealed class IoTControllerManager
    {
        private GpioController gpioController = null;
        public GpioController GpioController { get { return gpioController; } }

        private PwmController pwmController = null;
        public PwmController PwmController { get { return pwmController; } }


        public async Task InitializeAsync()
        {
            if (LightningProvider.IsLightningEnabled)
            {
                LowLevelDevicesController.DefaultProvider = LightningProvider.GetAggregateProvider();
                var gpioControllers = await GpioController.GetControllersAsync(LightningGpioProvider.GetGpioProvider());
                gpioController = gpioControllers[0];

                var pwmControllers = PwmController.GetControllersAsync(LightningPwmProvider.GetPwmProvider()).AsTask<IReadOnlyList<PwmController>>().Result;
                if (pwmControllers != null)
                    pwmController = pwmControllers[1];
            }
            else
            {
                gpioController = GpioController.GetDefault();
            }
        }

        public void SetPwmDesiredFrequency(double value)
        {
            if (pwmController == null)
                return;

            pwmController.SetDesiredFrequency(value);
        }
    }
}
