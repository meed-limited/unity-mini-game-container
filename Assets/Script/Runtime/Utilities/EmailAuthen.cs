using System;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

namespace SuperUltra.Container
{

    public static class EmailAuthen
    {
        public static void Login(string email, string password, Action successCallback = null, Action<string> errorCallback = null)
        {
            LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest()
            {
                Email = email, // Email address for the account
                Password = password, // Password for the new account
            };
            PlayFabClientAPI.LoginWithEmailAddress(
                request,
                (result) =>
                {
                    UserData.UpdatePlayFabId(result.PlayFabId);
                    SceneLoader.ToMenu();
                },
                (result) => { errorCallback(result.ErrorMessage); }
            );
        }

        public static void Register(string email, string password, Action successCallback = null, Action<string> errorCallback = null)
        {
            RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest()
            {
                Email = email, // Email address for the account
                Password = password, // Password for the new account
                RequireBothUsernameAndEmail = false // Require both userName and email to be passed to register an account
            };
            PlayFabClientAPI.RegisterPlayFabUser(
                request,
                (result) =>
                {
                    OnRegisterSuccess(result);
                    successCallback();
                },
                (result) => { 
                    OnRegisterFailure(result); 
                    errorCallback(result.ErrorMessage); 
                }
            );
        }

        public static void ForgotPassword(string email, Action successCallback = null, Action<string> errorCallback = null)
        {
            // TODO
            successCallback();
        }

        public static void Verify(string code, Action successCallback = null, Action<string> errorCallback = null)
        {
            // TODO
            successCallback();
        }
        
        public static void ResetPassword(string password, Action successCallback = null, Action<string> errorCallback = null)
        {
            // TODO
            successCallback();
        }

        static void OnRegisterSuccess(RegisterPlayFabUserResult result)
        {
            Debug.Log($"Register Successful {result.PlayFabId} {result.Username} {result}");
            UserData.UpdatePlayFabId(result.PlayFabId);
        }

        private static void OnRegisterFailure(PlayFabError error)
        {
            Debug.LogWarning("Something went wrong with your first API call.  :(");
            Debug.LogError("Here's some debug information:");
            Debug.LogError(error.GenerateErrorReport());
        }

    }

}