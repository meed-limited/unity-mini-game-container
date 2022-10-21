using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

namespace SuperUltra.Container
{

    public class AvatarSelectionUI : MonoBehaviour
    {
        [SerializeField] RectTransform _avatarPrefab;
        [SerializeField] RectTransform _avatarSelection;
        [SerializeField] Button _backButton;
        [SerializeField] UnityEvent _backButtonAction;
        [SerializeField] UnityEvent<Sprite> _avatarSelectedAction;
        [SerializeField] List<Sprite> _avatars = new List<Sprite>();
        int _numberOfElementInRow = 3;

        void Start()
        {
            if (_backButton != null)
            {
                _backButton.onClick.AddListener(() => _backButtonAction.Invoke());
            }
            CreateSelectionList();
        }

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

        public Sprite GetDefaultAvatar()
        {
            if(_avatars.Count > 0)
            {
                Sprite sprite = _avatars[0];
                return sprite;
            }
            return null;
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