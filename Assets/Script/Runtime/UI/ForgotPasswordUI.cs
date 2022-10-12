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

        bool CheckEmail(string email)
        {
            // use regex to check email format
            Regex validateEmailRegex = new Regex("^\\S+@\\S+\\.\\S+$");
            bool result = validateEmailRegex.IsMatch(email);
            if (!result)
            {
                _emailErrorText.text = "Email is not valid";
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
            _loginManager.OnClickForgotPassword(_email.text);
        }

    }

}