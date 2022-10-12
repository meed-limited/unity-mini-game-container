using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

namespace SuperUltra.Container
{
    
    public class FadeUI : MonoBehaviour
    {
        [SerializeField] bool _isVisible = false;
        [SerializeField] float animationTime = 0.33f;
        [SerializeField] UnityEvent _fadeOutCallBack;
        [SerializeField] UnityEvent _fadeInCallBack;
        Image _image;
        Color _originColor;
        Color _transparentColor;
        Button _button;
        CanvasGroup _canvasGroup;
        float _originAlpha;
        bool _isAnimating;

        void Awake()
        {
            Image image = GetComponent<Image>();
            Button button = GetComponent<Button>();
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

            SetCanvas(canvasGroup);
            SetButton(button);
            SetImage(image);
        }

        void SetCanvas(CanvasGroup canvasGroup)
        {
            _canvasGroup = canvasGroup;
            if (_canvasGroup != null)
            {
                _originAlpha = _canvasGroup.alpha;
                _canvasGroup.alpha = _isVisible ? _originAlpha : 0;
                _canvasGroup.blocksRaycasts = _isVisible;
            }
        }

        void SetButton(Button button)
        {
            _button = button;
            if (_button != null)
            {
                _button.interactable = _isVisible;
            }
        }

        void SetImage(Image image)
        {
            _image = image;
            if (_image != null)
            {
                _originColor = _image.color;
                _transparentColor = _image.color - new Color(0f, 0f, 0f, 1f);
                float visibleValue = _isVisible ? 1f : -1f;
                _image.color += new Color(0f, 0f, 0f, visibleValue);
                _image.raycastTarget = _isVisible;
            }
        }

        void ToggleButtonInteraction(bool isOn)
        {
            if (_button)
                _button.interactable = isOn;
        }

        Tween ImageAnimation(Color endValue)
        {
            if (_image == null)
                return null;

            return DOTween.To(
                () => _image.color,
                (Color value) => _image.color = value,
                endValue,
                animationTime
            );
        }

        Tween CanvasGroupAnimation(float endValue)
        {
            if (_canvasGroup == null)
                return null;

            return DOTween.To(
                () => _canvasGroup.alpha,
                (float value) => _canvasGroup.alpha = value,
                endValue,
                animationTime
            );
        }

        void SetInteraction(CanvasGroup canvasGroup, Image image, bool value)
        {
            if (canvasGroup)
            {
                canvasGroup.blocksRaycasts = value;
            }
            if (image)
            {
                image.raycastTarget = value;
            }
        }

        public Tween FadeIn()
        {
            if (_isAnimating)
            {
                return DOTween.Sequence();
            }
            _isAnimating = true;
            ToggleButtonInteraction(true);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(CanvasGroupAnimation(_originAlpha));
            sequence.Join(ImageAnimation(_originColor));
            sequence.OnComplete(
                () => { _isAnimating = false; }
            );
            _isVisible = true;
            SetInteraction(_canvasGroup, _image, true);
            _fadeInCallBack?.Invoke();

            return sequence;
        }

        public Tween FadeOut()
        {
            if (_isAnimating)
            {
                return DOTween.Sequence();
            }
            _isAnimating = true;
            Sequence sequence = DOTween.Sequence();

            sequence.Append(CanvasGroupAnimation(0));
            sequence.Join(ImageAnimation(_transparentColor));
            sequence.OnComplete(
                () =>
                {
                    ToggleButtonInteraction(false);
                    _isVisible = false;
                    SetInteraction(_canvasGroup, _image, false);
                    _isAnimating = false;
                    _fadeOutCallBack?.Invoke();
                }
            );

            return sequence;
        }

        public void Toggle()
        {
            Tween tween = _isVisible ? FadeOut() : FadeIn();
        }

    }
}
