namespace SuperUltra.Container
{
    public static class Config
    {

        #region Client
        
        public const float ReferenceScreenWidth = 1080f;

        public const float ReferenceScreenHeight = 2332f;

        #endregion
        
        #region Server
                
        public static string RemoteDevelopCatalogUrl = "http://192.168.56.1:61303";
        public static string RemoteStagingCatalogUrl = "https://ultra-game-board.s3.amazonaws.com";

        #endregion

        #region Build

#if UNITY_ANDROID
        public static string BuildTarget = "Android";
#elif UNITY_IOS
        public static string BuildTarget = "iOS";
#else
        public static string BuildTarget = "StandaloneWindows64";
#endif
        #endregion

    }

}
