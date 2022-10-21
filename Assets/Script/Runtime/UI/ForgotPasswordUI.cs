using System;
using UnityEngine;
using System.Text.RegularExpressions;
using TMPro;
using DG.Tweening;

namespace SuperUltra.Container
{

    public class ForgotPasswordUI : MonoBehaviour, ISlidable
    {
        [SerializeField] TMP_InputField _email;
        [SerializeField] TMP_Text _emailErrorText;
        [SerializeField] LoginManager _loginManager;
        [SerializeField] RectTransform _panel;
        Color _errorColor = new Color(0.96f, 0.4f, 0);
        Color _normalColor = new Color(0.2f, 0.2f, 0.2f);

        bool CheckEmail(string email)
        {
            // use regex to check email format
            Regex validateEmailRegex = new Regex("^\\S+@\\S+\\.\\S+$");
            bool result = validateEmailRegex.IsMatch(email);
            if (!result)
            {
                _emailErrorText.text = "Email is not valid";
                _emailErrorText.color = _errorColor;
                _email.targetGraphic.color = _errorColor;
            }
            return result;
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

        public void OnSubmit()
        {
            if (!CheckEmail(_email.text))
            {
                return;
            }
            _emailErrorText.text = "";
            _email.targetGraphic.color = _normalColor;
            _loginManager.OnClickForgotPassword(_email.text);
        }

    }

}