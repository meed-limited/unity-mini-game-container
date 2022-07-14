using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SuperUltra.Container
{
    
    public class LoginUI : MonoBehaviour
    {

        [SerializeField] TMP_InputField _emailInput;
        [SerializeField] TMP_InputField _passwordInput;
        [SerializeField] TMP_Text _errorText;
        [SerializeField] LoginManager _loginManager;

        public void OnClickEmailLogin()
        {
            if (CheckValidInput())
                _loginManager.OnClickEmailLogin(
                    _emailInput.text,
                    _passwordInput.text
                );
        }

        public void Back()
        {
            _loginManager.ToLoginSelection();
        }

        public void ForgotPassword()
        {
            _loginManager.ToForgotPasword();
        }

        bool CheckValidInput()
        {
            if (_emailInput.text.Length == 0 || _passwordInput.text.Length == 0)
            {
                _errorText.text = "Account info is not complete";
                return false;
            }
            return true;
        }


        public void OnClickFacebookLogin() => _loginManager.OnClickFacebookLogin();
        public void OnClickRegister() => _loginManager.ToEmailRegister();
    }

}