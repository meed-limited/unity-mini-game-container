using UnityEngine;
using UnityEngine.UI;
using System;

namespace SuperUltra.Container
{

    [Serializable]
    public static class UserData
    {
        public static string playFabId;
        public static string userName;
        public static Texture2D profilePic;
        public static string email;
        public static int totalTokenNumber;
        public static string walletAddress;
        public static int pointsInCurrentRank;
        public static int pointsToNextRank;
        public static int rankLevel;
        public static string rankTitle;
        public static void ClearData()
        {
            playFabId = "";
            userName = "";
            profilePic = null;
            email = "";
            totalTokenNumber = -1;
            walletAddress = "";
            pointsInCurrentRank = -1;
            pointsToNextRank = -1;
            rankLevel = -1;
            rankTitle = "";
        }
        
        public static void UpdateUserData(UpdateScoreResponseData data)
        {
            if (data == null)
                return;
            rankTitle = data.rankTitle;
            pointsToNextRank = data.pointsToNextRank;
            pointsInCurrentRank = data.experiencePoints;
        }
    }

}
