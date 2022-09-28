using UnityEngine;
using System;
using TMPro;

namespace SuperUltra.Container
{

    public class SettingUI : MonoBehaviour
    {
        [SerializeField] MenuManager _menuManager;
        public void ToFAQ() => Application.OpenURL(Config.FAQUrl);

        public void ToTermsAndCondition() => Application.OpenURL(Config.TermsUrl);

        public void ToPrivacy() => Application.OpenURL(Config.PrivacyUrl);

        public void SignOut()
        {
            LoadingUI.ShowInstance();
            NetworkManager.SignOut(
                () =>
                {
                    LoadingUI.HideInstance();
                    SceneLoader.ToLogin();
                }
            );
        }
    }

}