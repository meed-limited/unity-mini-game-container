using System;
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
        [SerializeField] TMP_InputField _name;
        [SerializeField] TMP_Text _statusText;
        [SerializeField] Button _submit;
        [SerializeField] LoginManager _loginManager;
        [SerializeField] RectTransform _panel;

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

        public void OnSubmit()
        {
            if (!(_name.text.Length >= 6 || _name.text.Length < 100))
            {
                _statusText.text = "Username must be between 6 and 99 characters";
                return;
            }
            
            PlayFabClientAPI.UpdateUserTitleDisplayName(
                new UpdateUserTitleDisplayNameRequest()
                {
                    DisplayName = _name.text
                },
                (result) =>
                {
                    PlayfabLogin.UpdateUserName(_name.text);
                    SceneLoader.ToMenu();
                },
                (error) =>
                {
                    _statusText.text = error.ErrorMessage;
                }
            );
            
        }
    }

}