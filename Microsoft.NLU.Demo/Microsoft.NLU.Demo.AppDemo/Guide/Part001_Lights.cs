using Microsoft.Cognitive.LUIS;
using Microsoft.NLU.Demo.Common.Accelerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Microsoft.NLU.Demo.AppDemo.Guide
{
    class Part001_Lights
    {
        private GpioManager gpioManager;
        private BingSpeechManager bingSpeechManager;
        private AudioPlaybackManager audioPlaybackManager;
        private OutputMessagesManager outputMessagesManager;
        private LuisManager luisManager;
        private WeatherServiceManager weatherServiceManager;

        public void Init(MediaElement AudioPlayer)
        {
            audioPlaybackManager = new AudioPlaybackManager(AudioPlayer);

            outputMessagesManager = new OutputMessagesManager(OutputMessagesManagerLanguage.pt_PT);
            bingSpeechManager = new BingSpeechManager("{BING API KEY}", BingSpeechManagerLanguage.pt_PT);
            luisManager = new LuisManager("LUIS ID", "LUIS KEY");

            gpioManager = new GpioManager();
            gpioManager.InitGpio();

            gpioManager.buttonPressed += GpioManager_buttonPressed;
            gpioManager.buttonReleased += GpioManager_buttonReleased;
        }


        private async Task StartCapture()
        {
            await bingSpeechManager.StartCapture();
            await audioPlaybackManager.PlayBeep();
        }

        private async Task StopCapture()
        {
            gpioManager.TurnLedOn();
            var textToSpeachResult = await bingSpeechManager.EndCapture();

            if (textToSpeachResult == null || textToSpeachResult.IsError)
            {
                await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetUnknownIntentOutput()));
            }
            else
            {
                await ProcessIntent(await luisManager.GetIntent(textToSpeachResult.SpokenText));
            }
            gpioManager.TurnLedOff();
        }

        private async Task ProcessIntent(LuisResult luisResult)
        {
            switch (luisResult.TopScoringIntent.Name.ToLowerInvariant())
            {
                case "lightsoff":
                    gpioManager.TurnLightOff();
                    await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetLightsOutIntentOutput()));
                    break;
                case "lightson":
                    gpioManager.TurnLightOn();
                    await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetLightsOnIntentOutput()));
                    break;
                case "none":
                default:
                    await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetUnknownIntentOutput()));
                    break;
            }
        }

        private async void GpioManager_buttonPressed(object sender, EventArgs e)
        {
            await StartCapture();
        }

        private async void GpioManager_buttonReleased(object sender, EventArgs e)
        {
            await StopCapture();
        }
    }
}
