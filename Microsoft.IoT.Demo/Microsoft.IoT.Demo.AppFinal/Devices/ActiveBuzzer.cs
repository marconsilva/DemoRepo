using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace Microsoft.IoT.Demo.AppFinal.Devices
{
    public sealed class ActiveBuzzer
    {
        private GpioPin outputPin = null;

        public void SetBuzzerState(bool isOn)
        {
            outputPin.Write(isOn ? GpioPinValue.High : GpioPinValue.Low);
        }

        public void Initialize(IoTControllerManager controllerManager, int pinNumber)
        {
            if (controllerManager == null || controllerManager.GpioController == null)
                return;
            var gpioController = controllerManager.GpioController;

            outputPin = gpioController.OpenPin(pinNumber);
            outputPin.SetDriveMode(GpioPinDriveMode.Output);
            outputPin.Write(GpioPinValue.Low);
        }
    }
}
