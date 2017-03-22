using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Devices.Tpm;
using Microsoft.Azure.Devices.Client;
using Windows.UI.Xaml;

namespace Microsoft.IoT.Demo.AppFinal.Cloud
{
    public sealed class AzureIoTHubManager
    {
        private DeviceClient deviceClient = null;
        private DispatcherTimer readMessagesTimer;

        public event EventHandler<IoTHhubEventRecievedMessage> MessageReceived;

        public AzureIoTHubManager()
        {
            readMessagesTimer = new DispatcherTimer();
            readMessagesTimer.Interval = TimeSpan.FromSeconds(1);
            readMessagesTimer.Tick += ReadMessagesTimer_Tick;
        }

        public void Init(uint deviceTpmId)
        {
            TpmDevice myDevice = new TpmDevice(deviceTpmId);

            deviceClient = DeviceClient.Create(
                myDevice.GetHostName(),
                AuthenticationMethodFactory.
                    CreateAuthenticationWithToken(myDevice.GetDeviceId(), myDevice.GetSASToken()), TransportType.Http1);

            if (deviceClient != null)
                readMessagesTimer.Start();
        }

        public async Task SendButtonEvent(bool isPressed)
        {
            string dataBuffer = Newtonsoft.Json.JsonConvert.SerializeObject(
                new { InputSensor = "Button", Value=isPressed ? "Pressed":"Relesed" });
            var eventMessage = new Message(Encoding.UTF8.GetBytes(dataBuffer));
            await deviceClient.SendEventAsync(eventMessage);
        }

        private async void ReadMessagesTimer_Tick(object sender, object e)
        {
            readMessagesTimer.Stop();
            try
            {
                Message receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage != null && MessageReceived != null)
                    MessageReceived.Invoke(this, new IoTHhubEventRecievedMessage() { Properties = receivedMessage.Properties });
                await deviceClient.CompleteAsync(receivedMessage);
            }
            catch { }
            finally
            {
                readMessagesTimer.Start();
            }

        }
        
    }
}
