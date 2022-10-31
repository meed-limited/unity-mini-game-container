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
            Action<bool> successCallback,
            Action<string> failureCallback
        )
        {
            // We invoke basic login procedure and pass in the callback to process the result
            FB.LogInWithReadPermissions(
                new List<string>() { "gaming_profile", "email" },
                (result) =>
                {
                    OnFacebookLoggedIn(result, successCallback, failureCallback);
                }
            );
        }

        private static void OnFacebookInitialized()
        {
            SetMessage("OnFacebookInitialized Finished");
        }

        private static void OnFacebookLoggedIn(
            ILoginResult fbLoginResult, 
            Action<bool> successCallback, 
            Action<string> failureCallback
        )
        {
            if (fbLoginResult == null)
            {
                SetMessage("fbLoginResult is null");
                failureCallback?.Invoke("");
                return;
            }

            // If result has no errors, it means we have authenticated in Facebook successfully
            if (string.IsNullOrEmpty(fbLoginResult.Error) && AccessToken.CurrentAccessToken != null)
            {
                SetMessage("Facebook Auth Complete! Access Token: " + AccessToken.CurrentAccessToken.TokenString + "\nLogging into PlayFab...");
                /*
                 * We proceed with making a call to PlayFab API. We pass in current Facebook AccessToken and let it create
                 * and account using CreateAccount flag set to true. We also pass the callback for Success and Failure results
                 */
                LoginPlayfabWithFacebook(successCallback, failureCallback);
            }
            else
            {
                if (AccessToken.CurrentAccessToken == null)
                {
                    SetMessage("Access Token is null");
                }

                // If Facebook authentication failed, we stop the cycle with the message
                SetMessage("Facebook Auth Failed: " + fbLoginResult.Error + "\n" + fbLoginResult.RawResult, true);
                failureCallback?.Invoke(fbLoginResult.Error);
            }
        }

        static void LoginPlayfabWithFacebook(Action<bool> successCallback, Action<string> failureCallback, bool isCreateAccount = false)
        {
            /*
            * We proceed with making a call to PlayFab API. We pass in current Facebook AccessToken and let it create
            * and account using CreateAccount flag set to true. We also pass the callback for Success and Failure results
            */
            PlayFabClientAPI.LoginWithFacebook(
                new LoginWithFacebookRequest { CreateAccount = isCreateAccount, AccessToken = AccessToken.CurrentAccessToken.TokenString },
                (result) =>
                {
                    OnPlayfabFacebookAuthComplete(result, isCreateAccount, successCallback, failureCallback);
                },
                (result) =>
                {
                    if (result.Error == PlayFabErrorCode.AccountNotFound)
                    {
                        LoginPlayfabWithFacebook(successCallback, failureCallback, true);
                        return;
                    }
                    OnPlayfabFacebookAuthFailed(result);
                    failureCallback(result.Error.ToString());
                }
            );
        }

        private static void OnPlayfabFacebookAuthComplete(LoginResult result, bool isCreateAccount, Action<bool> successCallback, Action<string> failureCallback)
        {
            SetMessage("PlayFab Facebook Auth Complete. Session ticket: " + result.SessionTicket);
            UserData.playFabId = result.PlayFabId;
            UserData.playFabSessionTicket = result.SessionTicket;
            successCallback(isCreateAccount);
        }

        private static void OnPlayfabFacebookAuthFailed(PlayFabError error)
        {
            SetMessage($"PlayFab Facebook Auth Failed: {error.Error}" + error.GenerateErrorReport(), true);
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