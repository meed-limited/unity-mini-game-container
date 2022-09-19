using System;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Text.RegularExpressions;
using UnityEngine.UI;

namespace SuperUltra.Container
{

    public class LoginUI : MonoBehaviour, ISlidable
    {

        [SerializeField] TMP_InputField _emailInput;
        [SerializeField] TMP_InputField _passwordInput;
        [SerializeField] TMP_Text _emailErrorText;
        [SerializeField] TMP_Text _passwordErrorText;
        [SerializeField] LoginManager _loginManager;
        [SerializeField] RectTransform _panel;
        Color _errorColor = new Color(0.96f, 0.4f, 0);
        Color _normalColor = new Color(0.4f, 0.45f, 0.52f);

        void Start()
        {
            GetSavedLoginCredential();
        }

        void GetSavedLoginCredential()
        {
            if(PlayerPrefs.HasKey(Config.CREDENTIAL_KEY_EMAIL))
            {
                _emailInput.text = PlayerPrefs.GetString(Config.CREDENTIAL_KEY_EMAIL);
            }
            if (PlayerPrefs.HasKey(Config.CREDENTIAL_KEY_PASSWORD))
            {
                _passwordInput.text = PlayerPrefs.GetString(Config.CREDENTIAL_KEY_PASSWORD);
            }
        }

        bool CheckValidInput()
        {

            if (!CheckEmail(_emailInput.text))
            {
                _emailErrorText.text = "Email is not valid";
                _emailErrorText.color = _errorColor;
                _emailInput.targetGraphic.color = _errorColor;
                return false;
            }
            
            _emailInput.targetGraphic.color = _normalColor;
            _emailErrorText.color = _normalColor;

            if (!CheckPassword(_passwordInput.text))
            {
                _passwordErrorText.text = "Password is empty";
                _passwordErrorText.color = _errorColor;
                _passwordInput.targetGraphic.color = _errorColor;
                return false;
            }

            _passwordInput.targetGraphic.color = _normalColor;
            _passwordErrorText.color = _normalColor;

            return true;
        }

        bool CheckEmail(string email)
        {
            // use regex to check email format
            Regex validateEmailRegex = new Regex("^\\S+@\\S+\\.\\S+$");
            bool result = validateEmailRegex.IsMatch(email);
            return result;
        }

        bool CheckPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;
            return true;
        }

        public void OnClickEmailLogin()
        {
            if (CheckValidInput())
                _loginManager.OnClickEmailLogin(
                    _emailInput.text,
                    _passwordInput.text
            );
        }

        public Tween SlideIn(float duration = 0.5f)
        {
            if (_panel == null) return null;
            return _panel.DOAnchorPos(Vector2.zero, duration);
        }

        public Tween SlideOut(float duration = 0.5f)
        {
            if (_panel == null) return null;
            return _panel.DOAnchorPos(new Vector2(0, -1920f), duration);
        }

        public void ChangeSlideDirection(SlideDirection direction) { }

        public void Back()
        {
            _loginManager.ToLoginSelection();
        }

        public void ForgotPassword()
        {
            _loginManager.ToForgotPasword();
        }

        public void OnClickFacebookLogin() => _loginManager.OnClickFacebookLogin();
        public void OnClickRegister() => _loginManager.ToRegister();


    }

}