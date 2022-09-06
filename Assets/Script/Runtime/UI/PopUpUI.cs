using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace SuperUltra.Container
{
    [RequireComponent(typeof(RectTransform))]
    public class PopUpUI : MonoBehaviour
    {
        RectTransform _rectTransform;
        [SerializeField] bool isShow = false;

        void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            if (isShow)
                _rectTransform.localScale = Vector3.one;
            else
                _rectTransform.localScale = Vector3.zero;
        }

        bool TryGetRectTransform(out RectTransform rectTransform)
        {
            rectTransform = GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Debug.LogError("PopUpUI requires a RectTransform component");
                return false;
            }
            return true;
        }

        public Tween Show(string message = "")
        {
            if (TryGetRectTransform(out RectTransform rectTransform))
            {
                _rectTransform = rectTransform;
            }
            else
            {
                return null;
            }

            return DOTween.To(
                () => _rectTransform.localScale,
                x => _rectTransform.localScale = x,
                new Vector3(1, 1, 1),
                0.2f
            );
        }

        public Tween Hide()
        {
            if (TryGetRectTransform(out RectTransform rectTransform))
            {
                _rectTransform = rectTransform;
            }
            else
            {
                return null;
            }

            return DOTween.To(
                () => _rectTransform.localScale,
                x => _rectTransform.localScale = x,
                new Vector3(0, 0, 0),
                0.2f
            );
        }

    }

}