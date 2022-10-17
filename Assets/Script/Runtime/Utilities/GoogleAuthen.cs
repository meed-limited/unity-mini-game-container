using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using PlayFab;
using PlayFab.ClientModels;
using System;


namespace SuperUltra.Container
{

    public static class GoogleAuthen
    {

        public static void Initialize()
        {
#if UNITY_ANDROID
            // The following grants profile access to the Google Play Games SDK.
            // Note: If you also want to capture the player's Google email, be sure to add
            // .RequestEmail() to the PlayGamesClientConfiguration
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            // .AddOauthScope("profile")
            .RequestServerAuthCode(false)
            .Build();
            PlayGamesPlatform.InitializeInstance(config);

            // recommended for debugging:
            PlayGamesPlatform.DebugLogEnabled = true;

            // Activate the Google Play Games platform
            PlayGamesPlatform.Activate();

#endif
        }

        public static void Login(Action<ResponseData> callback)
        {
            ResponseData data = new ResponseData { result = false };
#if UNITY_ANDROID
            PlayGamesPlatform.Instance.Authenticate((bool success) =>
            {

                if (success)
                {
                    // GoogleStatusText.text = "Google Signed In";
                    var serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                    Debug.Log("Server Auth Code: " + serverAuthCode);

                    PlayFabClientAPI.LoginWithGoogleAccount(
                        new LoginWithGoogleAccountRequest()
                        {
                            TitleId = PlayFabSettings.TitleId,
                            ServerAuthCode = serverAuthCode,
                            CreateAccount = true
                        }, 
                        (result) =>
                        {
                            // GoogleStatusText.text = "Signed In as " + result.PlayFabId;
                            data.message = "Signed In as " + result.PlayFabId;
                            callback?.Invoke(data);
                        }, 
                        (PlayFabError error) => {
                            data.message = error.ErrorMessage;
                            callback?.Invoke(data);
                        } 
                    );
                }
                else
                {
                    // GoogleStatusText.text = "Google Failed to Authorize your login";
                    data.message = "Google Failed to Authorize your login";
                    callback?.Invoke(data);
                }

            });
#endif
        }

    }

}

