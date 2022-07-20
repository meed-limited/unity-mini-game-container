using System;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace SuperUltra.Container
{
    
    public class RegisterUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField _emailInput;
        [SerializeField] TMP_InputField _passwordInput;
        [SerializeField] TMP_InputField _confirmPasswordInput;
        [SerializeField] TMP_Text _errorText;
        [SerializeField] LoginManager _loginManager;

        public void UpdateEmail(string email)
        {
            _loginManager.UpdateEmail(email);
        }

        public void UpdatePassword(string password)
        {
            _loginManager.UpdatePassword(password);
        }

        public void OnClickLogin() 
        {
            _loginManager.ToLoginSelection();
        } 

        public void OnSubmit()
        {
            if (CheckAccountInfo())
            {
                PlayFabLogin.RegisterWithEmail(
                    _loginManager.userData.email, 
                    _loginManager.userData.password, 
                    () => _loginManager.ToEnterUserName()
                );
            }
        }

        public void Back()
        {
            _loginManager.ToLoginSelection();
        }

        bool CheckPassword(string password, string confirmPassword)
        {
            if (!password.Equals(confirmPassword))
            {
                _errorText.text = "Password does not match";
                return false;
            }
            // check password has to be 6 to 100 character
            if (password.Length < 6 || confirmPassword.Length > 100)
            {
                _errorText.text = "Password must be 6 to 100 characters";
                return false;
            }
            else
            {
                _errorText.text = "";
            }
            return true;
        }

        bool CheckEmail(string email)
        {
            // use regex to check email format
            Regex validateEmailRegex = new Regex("^\\S+@\\S+\\.\\S+$");
            bool result = validateEmailRegex.IsMatch(email);
            if(!result)
            {
                _errorText.text = "Email is not valid";
            }
            return result;
        }

        bool CheckAccountInfo()
        {
            if (_loginManager.userData.email == null || _loginManager.userData.password == null)
            {
                _errorText.text = ("Account info is not complete");
                return false;
            }
            if(!CheckEmail(_loginManager.userData.email))
            {
                return false;
            }
            if (!CheckPassword(_passwordInput.text, _confirmPasswordInput.text))
            {
                return false;
            }
            _errorText.text = ("Account info is complete");
            return true;
        }
    
    }

}