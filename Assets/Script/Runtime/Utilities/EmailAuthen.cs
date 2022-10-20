using System;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

namespace SuperUltra.Container
{

    public static class EmailAuthen
    {
        public static void Login(string email, string password, Action<LoginResult> successCallback = null, Action<string> errorCallback = null)
        {
            LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest()
            {
                Email = email, // Email address for the account
                Password = password, // Password for the new account
            };
            PlayFabClientAPI.LoginWithEmailAddress(
                request,
                (LoginResult result) =>
                {
                    UserData.playFabId = result.PlayFabId;
                    successCallback(result);
                },
                (result) => { 
                    
                    Debug.Log("login error " + result.HttpCode + " " + result.HttpStatus + " " + result.ErrorMessage);
                    errorCallback(result.ErrorMessage); 
                }
            );
        }

        public static void Register(string email, string password, Action<string> successCallback = null, Action<string> errorCallback = null)
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
                    successCallback(result.PlayFabId);
                },
                (result) =>
                {
                    OnRegisterFailure(result);
                    errorCallback(result.ErrorMessage);
                }
            );
        }

        public static void ForgotPassword(string email, Action successCallback = null, Action<string> errorCallback = null)
        {
            // TODO
            NetworkManager.ForgetPasswordRequest(
                UserData.playFabId,
                (response) =>
                {
                    // if(response.result)
                    if (true)
                    {
                        successCallback?.Invoke();
                    }
                    else
                    {
                        errorCallback?.Invoke(response.message);
                    }
                }
            );
        }

        static void OnRegisterSuccess(RegisterPlayFabUserResult result)
        {
            Debug.Log($"Register Successful {result.PlayFabId} {result.Username} {result}");
            UserData.playFabId = result.PlayFabId;
            Debug.Log("OnRegisterSuccess " + UserData.playFabId);
        }

        private static void OnRegisterFailure(PlayFabError error)
        {
            Debug.LogWarning("Something went wrong with your first API call");
            Debug.LogWarning("Here's some debug information:");
            Debug.LogWarning(error.GenerateErrorReport());
        }

    }

}