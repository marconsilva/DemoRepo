using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace Microsoft.IoT.Demo.AppFinal.Devices
{
    public sealed class SimpleLed
    {
        private GpioPin outputPin = null;

        public void SetLedState(bool isOn)
        {
            outputPin.Write(isOn ? GpioPinValue.High : GpioPinValue.Low);
        }

        public void Initialize(IoTControllerManager controllerManager, int PinNumber)
        {
            if (controllerManager == null || controllerManager.GpioController == null)
                return;
            var gpioController = controllerManager.GpioController;

            outputPin = gpioController.OpenPin(PinNumber);
            outputPin.SetDriveMode(GpioPinDriveMode.Output);
        }
    }
}
