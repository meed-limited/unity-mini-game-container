using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using System;

namespace SuperUltra.Container
{

    public static class GoogleAuthen
    {

        public static void Login(Action<ResponseData> callback)
        {
            PlayGamesPlatform.Instance.Authenticate((status) =>
            {
                ProcessAuthentication(status, callback);
            });
        }

        static void ProcessAuthentication(SignInStatus status, Action<ResponseData> callback)
        {
            if (status == SignInStatus.Success)
            {
                string token = "";
                // Continue with Play Games Services
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log("Authorization code: " + code);
                    token = code;
                    // This token serves as an example to be used for SignInWithGooglePlayGames
                });
                callback?.Invoke(new ResponseData{
                    result = true,
                    message = $"{token}"
                });
            }
            else
            {
                // Disable your integration with Play Games Services or show a login button
                // to ask users to sign-in. Clicking it should call
                // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
                callback?.Invoke(new ResponseData
                {
                    result = false,
                    message = "Sign in failed"
                });
            }
        }

    }

}

