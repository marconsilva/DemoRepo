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
    class Part003_Weather
    {
        private GpioManager gpioManager;
        private BingSpeechManager bingSpeechManager;
        private AudioPlaybackManager audioPlaybackManager;
        private OutputMessagesManager outputMessagesManager;
        private LuisManager luisManager;
        private WeatherServiceManager weatherServiceManager;
        
        private async Task ProcessIntent(LuisResult luisResult)
        {
            switch (luisResult.TopScoringIntent.Name.ToLowerInvariant())
            {

                case "weatherforecast":
                    var forecast = await weatherServiceManager.GetCurrentWeather();
                    await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetForecastTypeMessage(forecast.Forecast)));
                    await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetForecastMinTemperatureMessage(forecast.MinTemperature)));
                    await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetForecastMaxTemperatureMessage(forecast.MaxTemperature)));
                    break;



                default:
                    await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetUnknownIntentOutput()));
                    break;
            }
        }
    }
}
