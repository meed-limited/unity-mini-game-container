using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperUltra.Container
{
    public class NewsUI : MonoBehaviour
    {

        public void ToHowToPlay() => Application.OpenURL(Config.HowToPlayUrl);
        public void ToMarketPlace() => Application.OpenURL(Config.MarketPlaceUrl);
        public void ToYoutube() => Application.OpenURL(Config.YotubeUrl);
        public void ToTwitter() => Application.OpenURL(Config.TwitterUrl);
        public void ToDiscord() => Application.OpenURL(Config.DiscordUrl);

    }

}
