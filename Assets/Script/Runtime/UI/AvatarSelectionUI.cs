using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

namespace SuperUltra.Container
{

    public class AvatarSelectionUI : MonoBehaviour, ISlidable
    {
        [SerializeField] RectTransform _panel;
        [SerializeField] RectTransform _avatarPrefab;
        [SerializeField] RectTransform _avatarSelection;
        [SerializeField] Button _backButton;
        [SerializeField] UnityEvent _backButtonAction;
        [SerializeField] UnityEvent<Sprite> _avatarSelectedAction;
        [SerializeField] List<Sprite> _avatars = new List<Sprite>();
        int _numberOfElementInRow = 3;
        Sprite _selectedAvatar;

        void Start()
        {
            _backButton.onClick.AddListener(() => _backButtonAction.Invoke());
            CreateSelectionList();
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

        void CreateSelectionList()
        {
            if (_avatarSelection == null || _avatarPrefab == null)
            {
                return;
            }

            foreach (Sprite item in _avatars)
            {
                CreatePrefab(item);
            }
            int rowCount = Mathf.CeilToInt((float)_avatars.Count / (float)_numberOfElementInRow);
            _avatarSelection.sizeDelta = new Vector2(0, _avatarPrefab.sizeDelta.y * rowCount);
        }

        void CreatePrefab(Sprite avatar)
        {
            RectTransform prefab = Instantiate(_avatarPrefab, _avatarSelection);
            prefab.GetComponentInChildren<Image>().sprite = avatar;
            prefab.GetComponent<Button>().onClick.AddListener(() =>
            {
                _avatarSelectedAction?.Invoke(avatar);
            });
        }
    }

}