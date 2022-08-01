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
            if (PlayfabLogin.userData.email == null || PlayfabLogin.userData.password == null)
            {
                _errorText.text = ("Account info is not complete");
                return false;
            }
            if(!CheckEmail(PlayfabLogin.userData.email))
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

        public void UpdateEmail(string email)
        {
            PlayfabLogin.UpdateEmail(email);
        }

        public void UpdatePassword(string password)
        {
            PlayfabLogin.UpdatePassword(password);
        }

        public void ToLoginPage()
        {
            _loginManager.ToLoginSelection();
        }

        public void ToTermsAndCondition() => Application.OpenURL("https://www.youtube.com/channel/UC-lHJZR3Gqxm24_Vd_AJ5Yw");
        
        public void ToPrivacy() => Application.OpenURL("https://www.facebook.com/PlayFab-Games-116495690789896/");

        public void OnClickRegister()
        {
            if (CheckAccountInfo())
            {
                _loginManager.OnClickEmailRegister();
            }
        }

        public void OnClickFacebookRegister() => _loginManager.OnClickFacebookRegister();


    }

}