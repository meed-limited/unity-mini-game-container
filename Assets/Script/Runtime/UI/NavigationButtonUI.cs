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
            Active, Deactive
        }

        [SerializeField] Button _button;
        [SerializeField] Image _activeMarker;
        [SerializeField] Image _icon;
        [SerializeField] TMP_Text _text;
        [SerializeField] bool _isActiveOnStart = false;
        [SerializeField] NavigationGroupUI _navigationGroupUI;
        float _animationTime = 0.3f;
        Vector2 _deactivePosition;
        Vector2 _activePosition;
        Vector2 _deactiveTextPosition;
        Vector2 _activeTextPosition;
        Color _enableColor = Color.white;
        Color _disableColor;
        State _state = State.Deactive;

        void Start()
        {
            CachePostiion();
            _disableColor = _text.color;
            if (_isActiveOnStart)
            {
                MoveAndResizeIcon(_activePosition, Vector3.one * 1.5f);
                ChangeTextColor(_enableColor);
                ChangeIconColor(_enableColor);
                _state = State.Active;
                _navigationGroupUI.Enable(this);
            }else
            {
                MoveText(new Vector2(0, -75f));
            }
        }

        void CachePostiion()
        {
            _deactivePosition = _icon.GetComponent<RectTransform>().anchoredPosition;
            _activePosition = _deactivePosition + new Vector2(0, 15f);
            _activeTextPosition = _text.GetComponent<RectTransform>().anchoredPosition;
            _deactiveTextPosition = _activeTextPosition + new Vector2(0, -75f);
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

        void MoveAndResizeIcon(Vector2 position, Vector3 scaleSize)
        {
            RectTransform rect = _icon.GetComponent<RectTransform>();
            rect.DOAnchorPos(
                position,
                _animationTime
            );
            rect.DOScale(scaleSize, _animationTime);
        }

        void MoveText(Vector2 position)
        {
            RectTransform rect = _text.GetComponent<RectTransform>();
            rect.DOAnchorPos(
                position,
                _animationTime
            );
        }

        void ChangeIconColor(Color color)
        {
            _icon.DOColor(
                color,
                _animationTime
            );
        }

        void ChangeTextColor(Color color)
        {
            _text.DOColor(
                color,
                _animationTime
            );
        }

        void MoveActiveMarker()
        {
            RectTransform markerRect = _activeMarker.GetComponent<RectTransform>();
            RectTransform rect = GetComponent<RectTransform>();
            markerRect.DOAnchorPos(
                rect.anchoredPosition,
                _animationTime
            );
        }

        public void Enable()
        {
            if(_state == State.Active) return;
            MoveActiveMarker();
            MoveAndResizeIcon(_activePosition, Vector3.one * 1.5f);
            MoveText(_activeTextPosition);
            ChangeTextColor(_enableColor);
            ChangeIconColor(_enableColor);
            _navigationGroupUI.Enable(this);
            _state = State.Active;
        }

        public void Disable()
        {
            if (_state == State.Deactive) return;
            MoveAndResizeIcon(_deactivePosition, Vector3.one);
            MoveText(_deactiveTextPosition);
            ChangeTextColor(_disableColor);
            ChangeIconColor(_disableColor);
            _state = State.Deactive;
        }

    }

}

