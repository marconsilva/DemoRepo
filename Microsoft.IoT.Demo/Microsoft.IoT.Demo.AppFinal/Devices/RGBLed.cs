using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Pwm;

namespace Microsoft.IoT.Demo.AppFinal.Devices
{
    public sealed class RGBLed
    {
        private PwmPin redOutputPin = null;
        private PwmPin greenOutputPin = null;
        private PwmPin blueOutputPin = null;
        private bool isLightOn = false;
        
        public static readonly double DefaultFrequency = 1000;
        public static readonly double DefaultActiveDutyCyclePercentage = 0.0;

        public void SetLedState(bool isOn)
        {
            if (isOn != isLightOn)
            {
                isLightOn = isOn;
                if (!isLightOn)
                {
                    SetOutputPinValue(0, redOutputPin);
                    SetOutputPinValue(0, greenOutputPin);
                    SetOutputPinValue(0, blueOutputPin);
                }
                else
                {
                    SetOutputPinValue(-1, redOutputPin);
                    SetOutputPinValue(-1, greenOutputPin);
                    SetOutputPinValue(-1, blueOutputPin);
                }
            }

        }

        public void SetLedColor(double redPercentage, double greenPercentage, double bluePercentage)
        {
            SetOutputPinValue(redPercentage, redOutputPin);
            SetOutputPinValue(greenPercentage, greenOutputPin);
            SetOutputPinValue(bluePercentage, blueOutputPin);
        }

        private void SetOutputPinValue(double percentage, PwmPin outputPin)
        {
            if(percentage >= 0)
                outputPin.SetActiveDutyCyclePercentage(percentage);

            if (isLightOn && !outputPin.IsStarted)
                outputPin.Start();
            else if (!isLightOn && outputPin.IsStarted)
                outputPin.Stop();
        }

        public void Initialize(IoTControllerManager controllerManager, int redPinNumber, int greenPinNumber, int bluePinNumber)
        {
            if (controllerManager == null || controllerManager.PwmController == null)
                return;
            var pwmController = controllerManager.PwmController;
            controllerManager.SetPwmDesiredFrequency(DefaultFrequency);

            redOutputPin = pwmController.OpenPin(redPinNumber);
            redOutputPin.SetActiveDutyCyclePercentage(DefaultActiveDutyCyclePercentage);
            greenOutputPin = pwmController.OpenPin(greenPinNumber);
            greenOutputPin.SetActiveDutyCyclePercentage(DefaultActiveDutyCyclePercentage);
            blueOutputPin = pwmController.OpenPin(bluePinNumber);
            blueOutputPin.SetActiveDutyCyclePercentage(DefaultActiveDutyCyclePercentage);
        }
    }
}
