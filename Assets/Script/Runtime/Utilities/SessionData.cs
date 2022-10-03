namespace SuperUltra.Container
{
    public static class SessionData
    {
        /// <summary>
        /// the game id that player is currently playing. -1 means player is not playing any game
        /// </summary>
        public static int currentGameId = -1;
        public static float currnetGameScore = -1;
        public static bool IsMusicOn;
        public static bool IsEffectSoundOn;
        static SessionData()
        {
            ContainerInterface.OnSetScore += OnSetScore;
            ContainerInterface.OnGetVolumeSetting += OnGetVolumeSetting;
        }

        static VolumeSetting OnGetVolumeSetting()
        {
            return new VolumeSetting { isMusicOn = IsMusicOn, isEffectOn = IsEffectSoundOn };
        }

        static void OnSetScore(float score)
        {
            SessionData.currnetGameScore = score;
        }

        public static void ClearData()
        {
            currentGameId = -1;
            currnetGameScore = -1;
            IsMusicOn = false;
            IsEffectSoundOn = false;
        }

    }
}