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

        public static void Login(Action<ResponseData> callback)
        {
            // The following grants profile access to the Google Play Games SDK.
            // Note: If you also want to capture the player's Google email, be sure to add
            // .RequestEmail() to the PlayGamesClientConfiguration
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .AddOauthScope("profile")
            .RequestServerAuthCode(false)
            .Build();
            PlayGamesPlatform.InitializeInstance(config);

            // recommended for debugging:
            PlayGamesPlatform.DebugLogEnabled = true;

            // Activate the Google Play Games platform
            PlayGamesPlatform.Activate();
        }

        private static void OnSignInButtonClicked()
        {
            Social.localUser.Authenticate((bool success) =>
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

                        }, 
                        (PlayFabError error) => {} 
                    );
                }
                else
                {
                    // GoogleStatusText.text = "Google Failed to Authorize your login";
                }

            });

        }

    }

}

