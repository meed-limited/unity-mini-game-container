using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

[RequireComponent(typeof(ScrollRect))]
public class StickyScrollUI : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    float _clampThreshHold;
    float _smoothTime = 0.2f;
    float _childSize;
    int _childCount = 0;
    float _nomalizedChildSize;
    Vector2 _currentPosition;
    Vector2 _lastPosition;
    [SerializeField] ScrollRect _scrollRect;
    [SerializeField] public UnityEvent<float> OnItemChange;

    void OnEnable()
    {
        _scrollRect = GetComponent<ScrollRect>();
        Initialize();
    }

    void Stick(bool intialScroll = false)
    {
        bool isOverTreshHold = false;

        if (_lastPosition.x < _currentPosition.x)
        {
            isOverTreshHold = Mathf.Abs(_currentPosition.x - _lastPosition.x) >= _clampThreshHold;
        }
        else
        {
            isOverTreshHold = Mathf.Abs(_currentPosition.x - _lastPosition.x) < _clampThreshHold;
        }

        _currentPosition.x = Mathf.Max(0, _currentPosition.x);
        int currentChild = Mathf.FloorToInt(_currentPosition.x / _nomalizedChildSize);
        float adjustOffset = isOverTreshHold ? currentChild + 1 : currentChild;
        float targetValue = _nomalizedChildSize * adjustOffset;
        targetValue = Mathf.Min(1, targetValue);
        DOTween.To(
            () => _scrollRect.horizontalNormalizedPosition,
            (float value) => _scrollRect.horizontalNormalizedPosition = value,
            targetValue,
            _smoothTime
        );
        OnItemChange?.Invoke(targetValue * (_childCount - 1));
    }

    public void ToChild(int targetPosition)
    {
        RectTransform content = _scrollRect.content;
        _currentPosition.x = targetPosition * _nomalizedChildSize;
        Stick(true);
    }

    public void Initialize()
    {
        _childSize = GetComponent<RectTransform>().rect.width;
        RectTransform content = _scrollRect.content;

        content.sizeDelta = new Vector2(_childSize * content.childCount, content.sizeDelta.y);

        _childCount = content.childCount;
        int childCount = _childCount - 1;

        if (childCount == 0)
            childCount = 1;
        _nomalizedChildSize = 1 / (float)childCount;
        _clampThreshHold = _nomalizedChildSize * 0.1f;
    }

    public void OnValueChanged(Vector2 value)
    {
        _currentPosition = value;
    }

    public void OnPointerDown(PointerEventData data)
    {
        _lastPosition = _currentPosition;
    }

    public void OnPointerUp(PointerEventData data)
    {
        Stick();
    }

}