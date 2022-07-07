namespace SuperUltra.Container
{
    public static class Config
    {
        public static string RemoteDevelopCatalogUrl = "http://192.168.56.1:61303";
        public static string RemoteStagingCatalogUrl = "https://ultra-game-board.s3.amazonaws.com";
#if UNITY_ANDROID
        public static string BuildTarget = "Android";
#elif UNITY_IOS
        public static string BuildTarget = "IOS";
#else
        public static string BuildTarget = "StandaloneWindows64";
#endif

    }

}
