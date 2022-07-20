using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System;

public static class PlayFabLogin
{
    // public void Start()
    // {
    //     if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
    //     {
    //         /*
    //         Please change the titleId below to your own titleId from PlayFab Game Manager.
    //         If you have already set the value in the Editor Extensions, this can be skipped.
    //         */
    //         PlayFabSettings.staticSettings.TitleId = "42A7F";
    //     }
    // }

    public static void RegisterWithEmail(string email, string password, Action callback)
    {
        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest();
        request.Email = email; // Email address for the account
        request.Password = password; // Password for the account
        request.RequireBothUsernameAndEmail = false; // Require either username and email to be valid; only one of these will be used to log on.
        PlayFabClientAPI.RegisterPlayFabUser(
            request,
            (result) =>
            {
                OnRegisterSuccess(result);
                callback();
            },
            OnLoginFailure
        );
    }

    public static void UpdateName(string name)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(
            new UpdateUserTitleDisplayNameRequest()
            {
                DisplayName = name
            },
            (result) =>
            {
                Debug.Log("Update name success");
            },
            (erorr) =>
            {
                Debug.Log("Update name failed");
                Debug.Log(erorr.GenerateErrorReport());
            }
        );
    }

    public static void Register()
    {
#if UNITY_ANDROID
        var request = new LoginWithAndroidDeviceIDRequest
        {
            AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
        };
        PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginSuccess, OnLoginFailure);
#elif UNITY_IOS
        var request = new LoginWithIOSDeviceIDRequest { 
            DeviceId = SystemInfo.deviceUniqueIdentifier, 
            CreateAccount = true,
        };
        PlayFabClientAPI.LoginWithIOSDeviceID(request , OnLoginSuccess, OnLoginFailure);
#else
        var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true };
        // PlayFabClientAPI (request, OnLoginSuccess, OnLoginFailure);
#endif
    }

    static void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log($"Register Successful {result.PlayFabId} {result.Username} {result}");
    }

    private static void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
    }

    private static void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
}