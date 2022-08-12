using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System;

namespace SuperUltra.Container
{

    [Serializable]
    public static class UserData
    {
        public static string userName { get; private set; }
        public static string email { get; private set; }
        public static string password { get; private set; }
        public static string token { get; private set; }
        public static string playFabId { get; private set; }

        public static void UpdateUserName(string name)
        {
            UserData.userName = name;
        }

        public static void UpdateEmail(string email)
        {
            UserData.email = email;
        }

        public static void UpdatePassword(string password)
        {
            UserData.password = password;
        }

        public static void UpdatePlayFabId(string id)
        {
            UserData.playFabId = id;
        }
    }

}
