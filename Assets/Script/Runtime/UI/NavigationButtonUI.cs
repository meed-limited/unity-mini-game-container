using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace SuperUltra.Container
{

    public class NavigationButtonUI : MonoBehaviour
    {
        enum State
        {
            Enabled, Disable
        }

        [SerializeField] Button _button;
        [SerializeField] Image _activeMarker;
        [SerializeField] Image _icon;
        [SerializeField] TMP_Text _text;
        [SerializeField] bool _isActiveOnStart = false;
        [SerializeField] NavigationGroupUI _navigationGroupUI;
        float _animationTime = 0.3f;
        State _state = State.Disable;

        void Start()
        {
            if (_isActiveOnStart)
            {
                MoveAndResizeIcon(new Vector2(0, 15f), Vector3.one * 1.5f);
                _state = State.Enabled;
                _navigationGroupUI.Enable(this);
            }else
            {
                MoveText(new Vector2(0, -50f));
            }
        }

        void Reset()
        {
            _button = GetComponent<Button>();
            foreach (Transform item in transform)
            {
                Image image = item.GetComponent<Image>();
                if (image) _icon = image;
            }
            _text = GetComponentInChildren<TMP_Text>();
        }

        void MoveAndResizeIcon(Vector2 offset, Vector3 scaleSize)
        {
            RectTransform rect = _icon.GetComponent<RectTransform>();
            rect.DOAnchorPos(
                rect.anchoredPosition + offset,
                _animationTime
            );
            rect.DOScale(scaleSize, _animationTime);
        }

        void MoveText(Vector2 offset)
        {
            RectTransform rect = _text.GetComponent<RectTransform>();
            rect.DOAnchorPos(
                rect.anchoredPosition + offset,
                _animationTime
            );
        }

        void MoveActiveMarker()
        {
            RectTransform markerRect = _activeMarker.GetComponent<RectTransform>();
            RectTransform rect = GetComponent<RectTransform>();
            Debug.Log(rect.anchoredPosition);
            markerRect.DOAnchorPos(
                rect.anchoredPosition,
                _animationTime
            );
        }

        public void Enable()
        {
            if(_state == State.Enabled) return;
            MoveActiveMarker();
            MoveAndResizeIcon(new Vector2(0, 15f), Vector3.one * 1.5f);
            MoveText(new Vector2(0, 50f));
            _navigationGroupUI.Enable(this);
            _state = State.Enabled;
        }

        public void Disable()
        {
            if (_state == State.Disable) return;
            MoveAndResizeIcon(new Vector2(0, -15f), Vector3.one);
            MoveText(new Vector2(0, -50f));
            _state = State.Disable;
        }

    }

}

