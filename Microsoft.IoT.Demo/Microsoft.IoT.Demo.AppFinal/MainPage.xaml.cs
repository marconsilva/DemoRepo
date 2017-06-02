using Microsoft.IoT.Demo.AppFinal.Cloud;
using Microsoft.IoT.Demo.AppFinal.Devices;
using Microsoft.IoT.Demo.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Microsoft.IoT.Demo.AppFinal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        IoTControllerManager controlerManager;
        SimpleLed simpleLed;
        SingleButton singleButton;
        ActiveBuzzer activeBuzzer;
        RGBLed rgbLed;

        AzureIoTHubManager azureIoTHubManager;

        Random random = new Random();
        bool isRGBLedOn = false;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            controlerManager = new IoTControllerManager();
            await controlerManager.InitializeAsync();

            simpleLed = new SimpleLed();
            simpleLed.Initialize(controlerManager, PinConfiguration.SimpleLedPin);
            simpleLed.SetLedState(false);

            singleButton = new SingleButton();
            singleButton.Initialize(controlerManager, PinConfiguration.SingleButtonPin);
            singleButton.Pressed += SingleButton_Pressed;
            singleButton.Released += SingleButton_Released;

            activeBuzzer = new ActiveBuzzer();
            activeBuzzer.Initialize(controlerManager, PinConfiguration.ActiveBuzzerPin);
            activeBuzzer.SetBuzzerState(false);

            rgbLed = new RGBLed();
            rgbLed.Initialize(controlerManager, PinConfiguration.RedRGBLedPin, PinConfiguration.BlueRGBLedPin, PinConfiguration.GreenRGBLedPin);
            rgbLed.SetLedState(false);

            azureIoTHubManager = new AzureIoTHubManager();
            azureIoTHubManager.Init(0);
            azureIoTHubManager.MessageReceived += AzureIoTHubManager_MessageReceived;


        }

        private void AzureIoTHubManager_MessageReceived(object sender, IoTHhubEventRecievedMessage e)
        {
            if (!e.Properties.ContainsKey("sensorName"))
                return;
            
            var sensorName = e.Properties["sensorName"];
            switch (sensorName.ToLowerInvariant())
            {
                case "led":
                    bool isOn = bool.Parse(e.Properties["value"]);
                    simpleLed.SetLedState(isOn);
                    break;
                case "buzzer":
                    activeBuzzer.SetBuzzerState(bool.Parse(e.Properties["value"]));
                    break;
                case "rgb":
                    if (e.Properties.ContainsKey("state"))
                        rgbLed.SetLedState(bool.Parse(e.Properties["state"]));
                    if (e.Properties.ContainsKey("red") && e.Properties.ContainsKey("green") && e.Properties.ContainsKey("blue"))
                        rgbLed.SetLedColor(double.Parse(e.Properties["red"]), double.Parse(e.Properties["green"]), double.Parse(e.Properties["blue"]));
                    break;
                default:
                    break;
            }
        }

        private async void SingleButton_Released(object sender, EventArgs e)
        {
            simpleLed.SetLedState(false);

            activeBuzzer.SetBuzzerState(false);

            isRGBLedOn = !isRGBLedOn;
            rgbLed.SetLedState(isRGBLedOn);

            await azureIoTHubManager.SendButtonEvent(false);
        }

        private async void SingleButton_Pressed(object sender, EventArgs e)
        {
            simpleLed.SetLedState(true);

            activeBuzzer.SetBuzzerState(true);

            rgbLed.SetLedColor(random.NextDouble(), random.NextDouble(), random.NextDouble());

            await azureIoTHubManager.SendButtonEvent(true);
        }
    }
}
