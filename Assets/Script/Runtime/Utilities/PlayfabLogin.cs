using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System;

namespace SuperUltra.Container
{

    [Serializable]
    public struct UserData
    {
        public string userName;
        public string email;
        public string password;
        public string token;
        public string playFabId;
    }

    public static class PlayfabLogin
    {
        static UserData _userData;
        public static UserData userData
        {
            get { return _userData; }
        }

        public static void GetPlayerInfo(Action<GetPlayerCombinedInfoResult> callback)
        {
            PlayFabClientAPI.GetPlayerCombinedInfo(
                new GetPlayerCombinedInfoRequest() {
                    InfoRequestParameters = new GetPlayerCombinedInfoRequestParams {GetPlayerProfile = true, GetUserAccountInfo = true },
                    PlayFabId = _userData.playFabId
                },
                (GetPlayerCombinedInfoResult result) => callback(result),
                (result) => Debug.Log(result.ErrorMessage)
            );
        }

        public static void UpdateUserName(string name)
        {
            _userData.userName = name;
        }

        public static void UpdateEmail(string email)
        {
            _userData.email = email;
        }

        public static void UpdatePassword(string password)
        {
            _userData.password = password;
        }

        public static void UpdatePlayFabId(string id)
        {
            _userData.playFabId = id;
        }

    }

}
