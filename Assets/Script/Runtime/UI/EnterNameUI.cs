using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using DG.Tweening;

namespace SuperUltra.Container
{

    public class EnterNameUI : MonoBehaviour, ISlidable
    {
        [SerializeField] TMP_InputField _nameInput;
        [SerializeField] TMP_Text _statusText;
        [SerializeField] Image _iconPreview;
        [SerializeField] Button _submit;
        [SerializeField] LoginManager _loginManager;
        [SerializeField] RectTransform _panel;
        Color _errorColor = new Color(0.96f, 0.4f, 0);
        Color _normalColor = new Color(0.2f, 0.2f, 0.2f);
        Vector2 _originPosition = Vector2.zero;

        void OnEnable()
        {
            _nameInput.onTouchScreenKeyboardStatusChanged.AddListener(OnTouchScreenKeyboardStatusChanged);
        }

        void OnDisable()
        {
            _nameInput.onTouchScreenKeyboardStatusChanged.RemoveListener(OnTouchScreenKeyboardStatusChanged);
        }

        public void Back()
        {
            _loginManager.ToLoginSelection();
        }

        public Tween SlideIn(float duration = 0.5f)
        {
            if (_panel == null) return null;
            return _panel.DOLocalMoveX(0, duration);
        }

        public Tween SlideOut(float duration = 0.5f)
        {
            if (_panel == null) return null;
            return _panel.DOLocalMoveX(Screen.height, duration);
        }

        public void ChangeSlideDirection(SlideDirection direction) { }

        void OnTouchScreenKeyboardStatusChanged(TouchScreenKeyboard.Status status)
        {
            if (status != TouchScreenKeyboard.Status.Visible)
            {
                OnInputDeselect();
            }
        }

        public void OnInputSelect()
        {
            _panel.anchoredPosition = _originPosition;
            Vector2 endValue = _panel.anchoredPosition + new Vector2(0, 260f);
            DOTween.To(
                () => _panel.anchoredPosition,
                (value) => _panel.anchoredPosition = value,
                endValue,
                0.3f
            );
        }

        void OnInputDeselect()
        {
            DOTween.To(
                () => _panel.anchoredPosition,
                (value) => _panel.anchoredPosition = value,
                _originPosition,
                0.3f
            );
        }

        public void SetAvatar(Sprite avatar)
        {
            _iconPreview.sprite = avatar;
        }

        public void OnSubmit()
        {
            if (!(_nameInput.text.Length >= 6 && _nameInput.text.Length < 100))
            {
                _statusText.text = "Username must be between 6 and 99 characters";
                _statusText.color = _errorColor;
                _nameInput.targetGraphic.color = _errorColor;
                return;
            }

            _statusText.text = "";
            _statusText.color = _normalColor;
            _nameInput.targetGraphic.color = _normalColor;
            
            LoadingUI.ShowInstance();
            PlayFabClientAPI.UpdateUserTitleDisplayName(
                new UpdateUserTitleDisplayNameRequest()
                {
                    DisplayName = _nameInput.text
                },
                (result) =>
                {
                    _statusText.text = "";
                    _statusText.color = _normalColor;
                    _nameInput.targetGraphic.color = _normalColor;
                    // _loginManager.ToMenu();
                },
                (error) =>
                {
                    LoadingUI.HideInstance();
                    _statusText.text = error.ErrorMessage;
                    _statusText.color = _errorColor;
                    _nameInput.targetGraphic.color = _errorColor;
                }
            );
            NetworkManager.UpdateUserProfile(
                UserData.playFabId,
                _nameInput.text,
                _iconPreview.sprite.texture,
                (ResponseData data) => {
                    if (!data.result)
                    {
                        LoadingUI.HideInstance();
                        MessagePopUpUI.Show($"Update user data failed\n{data.message}\nMark your user id\n{UserData.playFabId}");
                        return;
                    }
                    _loginManager.ToMenu();
                }
            );
            
        }

    }

}