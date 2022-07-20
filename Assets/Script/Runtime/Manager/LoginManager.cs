using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuperUltra.Container
{
    [Serializable]
    public struct UserData 
    {
        public string username;
        public string email;
        public string password;
        public string token;
    }    

    public class LoginManager : MonoBehaviour
    {
        static UserData _userData;
        [SerializeField] LoginUI _loginUI;
        [SerializeField] RegisterUI _registerUI;
        [SerializeField] EnterNameUI _enterNameUI;
        [SerializeField] ForgotPasswordUI _forgotPasswordUI;
        RectTransform _currentUI;
        public UserData userData { 
            get { return _userData; } 
        }

        void Start()
        {
            _loginUI.gameObject.SetActive(false);
            _registerUI.gameObject.SetActive(false);
            _enterNameUI.gameObject.SetActive(false);
            _forgotPasswordUI.gameObject.SetActive(false);
            ToLoginSelection();
            LoginWithToken();
        }

        public void LoginWithToken()
        {
            PlayerPrefs.HasKey("token");
            string token = PlayerPrefs.GetString("token");
        }

        public void OnClickFacebookLogin()
        {
            Debug.Log("Facebook login");
            FacebookAuthen.Login((bool result) => {
                if (result)
                {
                    Debug.Log("Facebook login success");
                    ToEnterUserName();
                }
                else
                {
                    Debug.Log("Facebook login failed");
                }
            });
        }

        public void OnClickEmailLogin(string email, string password)
        {
            Debug.Log("Email login");
            EmailAuthen.Login(email, password);
        }

        void ToPage(MonoBehaviour target)
        {
            if(_currentUI)
            {
                _currentUI.gameObject.SetActive(false);
            }
            target.gameObject.SetActive(true);
            _currentUI = target.GetComponent<RectTransform>();
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

        public void UpdateEmail(string email)
        {
            _userData.email = email;
        }

        public void UpdatePassword(string password)
        {
            _userData.password = password;
        }

        public void UpdateUsername(string username)
        {
            _userData.username = username;
        }

    }

}