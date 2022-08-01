using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperUltra.Container
{

    public class LoginManager : MonoBehaviour
    {
        static UserData _userData;
        [SerializeField] LoginUI _loginUI;
        [SerializeField] RegisterUI _registerUI;
        [SerializeField] EnterNameUI _enterNameUI;
        [SerializeField] ForgotPasswordUI _forgotPasswordUI;
        [SerializeField] MessagePopUpUI _messagePopUpUI;
        RectTransform _currentUI;

        void ToPage(MonoBehaviour target)
        {
            if (_currentUI)
            {
                _currentUI.gameObject.SetActive(false);
            }
            target.gameObject.SetActive(true);
            _currentUI = target.GetComponent<RectTransform>();
        }

        bool CheckInternetConnection()
        {
            if (
                !Application.internetReachability.Equals(NetworkReachability.ReachableViaLocalAreaNetwork)
                && !Application.internetReachability.Equals(NetworkReachability.ReachableViaCarrierDataNetwork)
            )
            {
                _messagePopUpUI.Show("No Connection", "Retry", () => { CheckInternetConnection(); }, false);
                return false;
            }
            return true;
        }

        void Start()
        {
            FacebookAuthen.Initialize();
            _loginUI.gameObject.SetActive(false);
            _registerUI.gameObject.SetActive(false);
            _enterNameUI.gameObject.SetActive(false);
            _forgotPasswordUI.gameObject.SetActive(false);
            _messagePopUpUI.gameObject.SetActive(false);
            if (!CheckInternetConnection())
            {
                return;
            }
            ToLoginSelection();
            AutoLoginWithToken();
        }

        public void ReTryConnection()
        {
            Start();
        }

        public void AutoLoginWithToken()
        {
            if (PlayerPrefs.HasKey("token"))
            {
                string token = PlayerPrefs.GetString("token");
            }
        }

        public void OnClickFacebookLogin()
        {
            FacebookAuthen.Login(
                () => { SceneLoader.ToMenu(); },
                (string errorMessage) =>
                {
                    _messagePopUpUI.gameObject.SetActive(true);
                    _messagePopUpUI.Show(errorMessage, "OK", null, true);
                },
                false
            );
        }

        public void OnClickFacebookRegister()
        {
            FacebookAuthen.Login(
                () => { ToEnterUserName(); },
                (string errorMessage) =>
                {
                    _messagePopUpUI.gameObject.SetActive(true);
                    _messagePopUpUI.Show(errorMessage, "OK", null, true);
                },
                true
            );
        }

        public void OnClickEmailLogin(string email, string password)
        {
            EmailAuthen.Login(email, password, null, (string errorMessage) =>
            {
                _messagePopUpUI.Show(errorMessage, "OK", null, true);
            });
        }

        public void OnClickEmailRegister()
        {
            EmailAuthen.Register(
                PlayfabLogin.userData.email,
                PlayfabLogin.userData.password,
                () => ToEnterUserName(),
                (string errorMessage) =>
                {
                    _messagePopUpUI.Show(errorMessage, "OK", null, true);
                }
            );
        }

        public void ToRegister()
        {
            ToPage(_registerUI);
        }

        public void ToLoginSelection()
        {
            ToPage(_loginUI);
        }

        public void ToEnterUserName()
        {
            ToPage(_enterNameUI);
        }

        public void ToForgotPasword()
        {
            ToPage(_forgotPasswordUI);
        }

    }

}