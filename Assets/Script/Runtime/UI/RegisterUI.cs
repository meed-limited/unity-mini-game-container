using System;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

namespace SuperUltra.Container
{

    public class RegisterUI : MonoBehaviour, ISlidable
    {
        [SerializeField] TMP_InputField _emailInput;
        [SerializeField] TMP_InputField _passwordInput;
        [SerializeField] TMP_InputField _confirmPasswordInput;
        [SerializeField] TMP_Text _passwordErrorText;
        [SerializeField] TMP_Text _emailErrorText;
        [SerializeField] Toggle _termsAndConditionsToggle;
        [SerializeField] TMP_Text _termsAndConditionsErrorText;
        [SerializeField] LoginManager _loginManager;
        [SerializeField] RectTransform _panel;
        Color _errorColor = new Color(0.96f, 0.4f, 0);
        Color _normalColor = new Color(0.4f, 0.45f, 0.52f);

        bool CheckPassword(string password, string confirmPassword)
        {
            Regex validatePasswordRegex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])");
            bool result = validatePasswordRegex.IsMatch(password);
            if (!result)
            {
                _passwordErrorText.text = "Use 8 or more characters with mix of leters & numbers";
                _passwordErrorText.color = _errorColor;
                _passwordInput.targetGraphic.color = _errorColor;
                return false;
            }
            if (!password.Equals(confirmPassword))
            {
                _passwordErrorText.text = "Password does not match";
                _passwordErrorText.color = _errorColor;
                _passwordInput.targetGraphic.color = _errorColor;
                _confirmPasswordInput.targetGraphic.color = _errorColor;
                return false;
            }
            _passwordErrorText.text = "";
            _passwordInput.targetGraphic.color = _normalColor;
            _confirmPasswordInput.targetGraphic.color = _normalColor;
            return true;
        }

        bool CheckEmail(string email)
        {
            // use regex to check email format
            Regex validateEmailRegex = new Regex("^\\S+@\\S+\\.\\S+$");
            bool result = validateEmailRegex.IsMatch(email);
            _emailErrorText.text = result ? "" : "Email is not valid";
            _emailErrorText.color = result ? _normalColor : _errorColor;
            return result;
        }

        bool CheckAccountInfo()
        {
            if (!CheckEmail(_emailInput.text))
            {
                return false;
            }
            if (!CheckPassword(_passwordInput.text, _confirmPasswordInput.text))
            {
                return false;
            }
            return true;
        }

        bool CheckTermsAndConditions()
        {
            if (!_termsAndConditionsToggle.isOn)
            {
                _termsAndConditionsErrorText.text = "Please accept terms and conditions";
                _termsAndConditionsErrorText.color = _errorColor;
                return false;
            }
            _termsAndConditionsErrorText.text = "";
            _termsAndConditionsErrorText.color = _normalColor;
            return true;
        }

        public void ToLoginPage()
        {
            _loginManager.ToLoginSelection();
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

        public void ChangeSlideDirection(SlideDirection direction) {}

        public void ToFAQ() => Application.OpenURL(Config.FAQUrl);

        public void ToTermsAndCondition() => Application.OpenURL(Config.TermsUrl);

        public void ToPrivacy() => Application.OpenURL(Config.PrivacyUrl);

        public void OnClickRegister()
        {
            if (CheckAccountInfo() && CheckTermsAndConditions())
            {
                _loginManager.OnClickEmailRegister(_emailInput.text, _passwordInput.text);
            }
        }

        public void OnClickFacebookRegister() => _loginManager.OnClickFacebookRegister();


    }

}