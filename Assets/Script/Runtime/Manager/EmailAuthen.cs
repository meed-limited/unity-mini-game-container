using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SuperUltra.Container
{

    public class EmailAuthen
    {
        public static void Login(string name, string password)
        {
            // TODO : make calls to server to login
            SceneLoader.ToMenu();
        }

        public static void Register()
        {
            SceneLoader.ToMenu();
        }
    }

}