using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;

namespace Microsoft.IoT.Demo.AppFinal.Devices
{
class SingleButton
{
    private enum ButtonState { Released = 0, Pressed = 1 }

    public event EventHandler<EventArgs> Pressed;
    public event EventHandler<EventArgs> Released;
    private readonly long INTERVAL_IN_TICKS = 5000;
    private DispatcherTimer readButtonState = null;
    private GpioPin inputPin = null;
    private ButtonState currentButtonValue;


    public SingleButton()
    {
        readButtonState = new DispatcherTimer();
        readButtonState.Tick += ReadButtonState_Tick;
    }

    private void ReadButtonState_Tick(object sender, object e)
    {
        if (inputPin == null)
            return;

        ButtonState newButtonValue = ReadValue();

        if (newButtonValue != currentButtonValue)
        {
            currentButtonValue = newButtonValue;
            var isPressed = currentButtonValue == ButtonState.Pressed ? true : false;

            if (isPressed && Pressed != null)
                Pressed.Invoke(this, null);
            else if (!isPressed && Released != null)
                Released.Invoke(this, null);
        }
    }

    private ButtonState ReadValue()
    {
        return inputPin.Read() == GpioPinValue.Low
            ? ButtonState.Pressed : ButtonState.Released;
    }

    public void Initialize(IoTControllerManager controllerManager, int pinNumber)
    {
        if (controllerManager == null || controllerManager.GpioController == null)
            return;
        var gpioController = controllerManager.GpioController;

        inputPin = gpioController.OpenPin(pinNumber);
        inputPin.SetDriveMode(GpioPinDriveMode.Input);

        readButtonState.Interval = new TimeSpan(INTERVAL_IN_TICKS);
        readButtonState.Start();
    }
}
}
