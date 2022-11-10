using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace SuperUltra.Container
{

    public class EditProfileUI : MonoBehaviour
    {
        [SerializeField] Image _avatarPreview;
        [SerializeField] TMP_InputField _userName;
        [SerializeField] MenuManager _menuManager;
        [SerializeField] RectTransform _panel;
        Texture2D _previewTexture;
        Vector2 _originPosition = Vector2.zero;

        void OnEnable()
        {
            _userName.onTouchScreenKeyboardStatusChanged.AddListener(OnTouchScreenKeyboardStatusChanged);
        }

        void OnDisable()
        {
            _userName.onTouchScreenKeyboardStatusChanged.RemoveListener(OnTouchScreenKeyboardStatusChanged);
        }


        void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            SetAvatar(_previewTexture != null ? _previewTexture : UserData.profilePic);
            SetUserName(UserData.userName);
        }

        void SetUserName(string name)
        {
            if (_userName != null)
            {
                _userName.text = name;
            }
        }

        void SetAvatar(Texture2D texture)
        {
            if (_avatarPreview && texture)
            {
                _avatarPreview.sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
            }
        }

        void OnTouchScreenKeyboardStatusChanged(TouchScreenKeyboard.Status status)
        {
            if (status != TouchScreenKeyboard.Status.Visible)
            {
                OnInputDeselect();
            }
        }

        public void OnInputSelect(float offset)
        {
            _panel.anchoredPosition = _originPosition;
            Vector2 endValue = _panel.anchoredPosition + new Vector2(0, offset);
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

        public void Back()
        {
            _menuManager.ToProfilePage();
            _previewTexture = null;
        }

        public void SetPreviewTexture(Sprite sprite)
        {
            _previewTexture = sprite.texture;
        }

        public void SaveProfile()
        {
            if (_avatarPreview && _avatarPreview.sprite && _userName)
            {
                bool isAvatarChanged = !_avatarPreview.sprite.texture.Equals(UserData.profilePic);

                if (isAvatarChanged)
                {
                    _menuManager.UpdateUserProfileRequest(_userName.text, _avatarPreview.sprite.texture);
                }
                else
                {
                    _menuManager.UpdateUserNameRequest(_userName.text);
                }

                _previewTexture = null;
            }
        }

    }

}