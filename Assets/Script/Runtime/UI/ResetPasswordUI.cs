using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace SuperUltra.Container
{
    public class ResetPasswordUI : MonoBehaviour, ISlidable
    {
        [SerializeField] LoginManager _loginManager;
        [SerializeField] TMP_InputField _password;
        [SerializeField] TMP_InputField _confirmPassword;
        [SerializeField] TMP_Text _passwordErrorText;
        [SerializeField] RectTransform _panel;
        Color _errorColor = new Color(0.96f, 0.4f, 0);
        Color _normalColor = new Color(0.4f, 0.45f, 0.52f);

        public void OnClickResetPassword()
        {
            if (!CheckPassword(_password.text, _confirmPassword.text))
            {
                return;
            }
            _loginManager.OnClickResetPassword(_password.text);
        }

        bool CheckPassword(string password, string confirmPassword)
        {
            Regex validatePasswordRegex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\\$%\\^&\\*])(?=.{8,})");
            bool result = validatePasswordRegex.IsMatch(password);
            if (!result)
            {
                _passwordErrorText.text = "Use 8 or more characters with a mix of leter & symbols";
                _passwordErrorText.color = _errorColor;
                _password.targetGraphic.color = _errorColor;
                return false;
            }
            if (!password.Equals(confirmPassword))
            {
                _passwordErrorText.text = "Password does not match";
                _passwordErrorText.color = _errorColor;
                _password.targetGraphic.color = _errorColor;
                _confirmPassword.targetGraphic.color = _errorColor;
                return false;
            }
            _passwordErrorText.text = "";
            _password.targetGraphic.color = _normalColor;
            _confirmPassword.targetGraphic.color = _normalColor;
            return true;
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

    }
    
}


