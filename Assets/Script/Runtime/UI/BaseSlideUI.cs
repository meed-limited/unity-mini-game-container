using System;
using UnityEngine;
using DG.Tweening;

namespace SuperUltra.Container
{
    public enum SlideDirection { Left, Right, Up, Down }
    public class BaseSlideUI : MonoBehaviour, ISlidable
    {

        [SerializeField] RectTransform _panel;
        [Tooltip("The slide in direction (from the oppsite direction)")]
        [SerializeField] SlideDirection _direction;

        Vector2 GetSlideInPosition()
        {
            switch (_direction)
            {
                case SlideDirection.Left:
                    return new Vector2(-Config.REFERENCE_SCREEN_WIDTH, 0);
                case SlideDirection.Right:
                    return new Vector2(Config.REFERENCE_SCREEN_WIDTH, 0);
                case SlideDirection.Up:
                    return new Vector2(0, Config.REFERENCE_SCREEN_HEIGHT);
                case SlideDirection.Down:
                    return new Vector2(0, -Config.REFERENCE_SCREEN_HEIGHT);
                default:
                    return Vector2.zero;
            }
        }

        public Tween SlideIn(float duration = 0.5f)
        {
            if (_panel == null) return null;
            _panel.anchoredPosition = GetSlideInPosition();
            return _panel.DOAnchorPos(Vector2.zero, duration);
        }

        public Tween SlideOut(float duration = 0.5f)
        {
            if (_panel == null) return null;
            Vector2 target = GetSlideInPosition();
            return _panel.DOAnchorPos(target, duration);
        }

        public void ChangeSlideDirection(SlideDirection direction)
        {
            _direction = direction;
        }

    }
}