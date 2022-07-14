using System;
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
                _loginManager.ToEnterUserName();
        }
        
        public void Back()
        {
            _loginManager.ToLoginSelection();
        }

        bool CheckPassword()
        {
            if (_passwordInput.text != _confirmPasswordInput.text)
            {
                _errorText.text = "Password does not match";
                return false;
            }
            else
            {
                _errorText.text = "";
            }
            return true;
        }

        bool CheckAccountInfo()
        {
            if (_loginManager.userData.email == null || _loginManager.userData.password == null)
            {
                _errorText.text = ("Account info is not complete");
                return false;
            }
            if (!CheckPassword())
            {
                _errorText.text = ("Password does not match");
                return false;
            }
            _errorText.text = ("Account info is complete");
            return true;
        }
    
    }

}