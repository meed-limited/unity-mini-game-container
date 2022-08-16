using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace SuperUltra.Container
{

    public class VerifyUI : MonoBehaviour, ISlidable
    {
        [SerializeField] TMP_InputField _codeOne;
        [SerializeField] TMP_InputField _codeTwo;
        [SerializeField] TMP_InputField _codeThree;
        [SerializeField] TMP_InputField _codeFour;
        [SerializeField] TMP_Text _codeErrorText;
        [SerializeField] LoginManager _loginManager;
        [SerializeField] RectTransform _panel;

        public void OnSubmit()
        {
            if (!CheckCode())
            {
                return;
            }
            _codeErrorText.text = "";
            _codeOne.text = "";
            _codeTwo.text = "";
            _codeThree.text = "";
            _codeFour.text = "";
            _loginManager.OnClickVerify($"{_codeOne.text}{_codeTwo.text}{_codeThree.text}{_codeFour.text}");
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

        bool CheckCode()
        {
            if (_codeOne.text.Length == 0 || _codeTwo.text.Length == 0 || _codeThree.text.Length == 0 || _codeFour.text.Length == 0)
            {
                _codeErrorText.text = "Code is not valid";
                return false;
            }
            return true;
        }
    }

}
