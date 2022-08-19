// Import statements introduce all the necessary classes for this example.
using Facebook.Unity;
using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using LoginResult = PlayFab.ClientModels.LoginResult;

namespace SuperUltra.Container
{

    public static class FacebookAuthen
    {

        public static void Initialize()
        {
            FB.Init(OnFacebookInitialized);
        }

        public static void Login(
            Action successCallback,
            Action<string> failureCallback, 
            bool isCreateAccount = true
        )
        {
            // We invoke basic login procedure and pass in the callback to process the result
            FB.LogInWithReadPermissions(
                new List<string>() { "gaming_profile", "email" },
                (result) =>
                {
                    OnFacebookLoggedIn(result, successCallback, failureCallback, isCreateAccount);
                }
            );
        }

        private static void OnFacebookInitialized()
        {
            SetMessage("OnFacebookInitialized Finished");
        }

        private static void OnFacebookLoggedIn(
            ILoginResult result, 
            Action successCallback, 
            Action<string> failureCallback,
            bool isCreateAccount = true
        )
        {
            // If result has no errors, it means we have authenticated in Facebook successfully
            if (result == null || string.IsNullOrEmpty(result.Error))
            {
                SetMessage("Facebook Auth Complete! Access Token: " + AccessToken.CurrentAccessToken.TokenString + "\nLogging into PlayFab...");
                foreach (var item in result.ResultDictionary)
                {
                    Debug.Log(item);
                }
                /*
                 * We proceed with making a call to PlayFab API. We pass in current Facebook AccessToken and let it create
                 * and account using CreateAccount flag set to true. We also pass the callback for Success and Failure results
                 */
                PlayFabClientAPI.LoginWithFacebook(
                    new LoginWithFacebookRequest { CreateAccount = isCreateAccount, AccessToken = AccessToken.CurrentAccessToken.TokenString },
                    (result) => {
                        OnPlayfabFacebookAuthComplete(result);
                        successCallback();
                    }, 
                    (result) => {
                        OnPlayfabFacebookAuthFailed(result);
                        failureCallback(result.Error.ToString());
                    }
                );
            }
            else
            {
                // If Facebook authentication failed, we stop the cycle with the message
                SetMessage("Facebook Auth Failed: " + result.Error + "\n" + result.RawResult, true);
                failureCallback(result.Error);
            }
        }

        // When processing both results, we just set the message, explaining what's going on.
        private static void OnPlayfabFacebookAuthComplete(LoginResult result)
        {
            SetMessage("PlayFab Facebook Auth Complete. Session ticket: " + result.SessionTicket);
            UserData.playFabId = result.PlayFabId;
        }

        private static void OnPlayfabFacebookAuthFailed(PlayFabError error)
        {
            SetMessage("PlayFab Facebook Auth Failed: " + error.GenerateErrorReport(), true);
        }

        public static void SetMessage(string message, bool error = false)
        {
            if (error)
                Debug.LogError(message);
            else
                Debug.Log(message);
        }


    }

}