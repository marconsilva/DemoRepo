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
    class Part005_WhoIs
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


                case "whois":
                    foreach (var entityKeyMatch in luisResult.Entities)
                    {
                        foreach (var entity in entityKeyMatch.Value)
                        {
                            await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetWhoIsMessage(entityKeyMatch.Key, entity.Value)));
                        }
                    }
                    break;


                default:
                    await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetUnknownIntentOutput()));
                    break;
            }
        }
    }
}
