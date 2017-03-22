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
    class Part002_PlayMusic
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
                case "playmusic":
                    if (!luisResult.Entities.ContainsKey("SongName"))
                        await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetMusicNameNotFoundMessageOutput()));
                    else
                    {
                        switch (luisResult.Entities["SongName"].FirstOrDefault().Value.ToLowerInvariant())
                        {
                            case "born in the u":
                            case "born in the usa":
                                await audioPlaybackManager.PlayMusic001();
                                break;
                            case "eye of the tiger":
                                await audioPlaybackManager.PlayMusic002();
                                break;
                            default:
                                await audioPlaybackManager.PlayAudio(await bingSpeechManager.GetAudio(outputMessagesManager.GetMusicNotFoundMessageOutput()));
                                break;
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
