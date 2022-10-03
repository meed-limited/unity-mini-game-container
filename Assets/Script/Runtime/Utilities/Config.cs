namespace SuperUltra.Container
{
    public static class Config
    {
        public const string CREDENTIAL_KEY_EMAIL = "CREDENTIAL_KEY_EMAIL";
        public const string CREDENTIAL_KEY_PASSWORD = "CREDENTIAL_KEY_PASSWORD";
        public const string CREDENTIAL_KEY_PLAYFAB_ID = "CREDENTIAL_KEY_PLAYFAB_ID";
        public const string KEY_SOUND = "KEY_SOUND";
        public const string KEY_MUSIC = "KEY_MUSIC";

        #region Client

        public const float REFERENCE_SCREEN_WIDTH = 1080f;
        public const float REFERENCE_SCREEN_HEIGHT = 2332f;
        public const float WithDrawLimit = 750f;

        #endregion
        
        #region Server
                
        public const string RemoteDevelopCatalogUrl = "http://192.168.56.1:61303";
        public const string RemoteStagingCatalogUrl = "https://ultra-game-board.s3.amazonaws.com";
        public const string Domain = "https://ultranova.lepricon.city/api/v1/platform/";
        public const string GitBookUrl = "https://superultra.gitbook.io/ultranova";
        public const string FAQUrl = GitBookUrl + "/";
        public const string TermsUrl = GitBookUrl + "/terms-and-conditions";
        public const string PrivacyUrl = GitBookUrl + "/privacy-policy";
        public const string HowToPlayUrl = GitBookUrl + "/faq/how-to-play";
        public const string HowToWithDrawalUrl = GitBookUrl + "/faq/withdrawal";
        public const string YotubeUrl = "https://www.youtube.com/channel/UCMstYEJboRs6K096AP_sz8Q";
        public const string TwitterUrl = "https://twitter.com/ultranova_app";
        // TODO
        public const string DiscordUrl = "https://twitter.com/ultranova_app";
        // TODO 
        public const string MarketPlaceUrl = "https://twitter.com/ultranova_app";
        // TODO
        public const string WithDrawUrl = "https://main.d41x8g6yyhv7w.amplifyapp.com/dashboard";

        #endregion

        #region Build

#if UNITY_ANDROID
        public const string BuildTarget = "Android";
#elif UNITY_IOS
        public const string BuildTarget = "iOS";
#else
        public const string BuildTarget = "StandaloneWindows64";
#endif
        #endregion

    }

}
