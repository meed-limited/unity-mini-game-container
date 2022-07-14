using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SuperUltra.Container
{

    public class FacebookAuthen
    {
        public static void Login(Action<bool> loginCallback)
        {
            bool result = true;
            loginCallback(result);
        }

        public static void Register()
        {
            SceneLoader.ToMenu();
        }
    }

}