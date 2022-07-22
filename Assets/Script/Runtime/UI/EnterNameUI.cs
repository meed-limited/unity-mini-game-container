using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

namespace SuperUltra.Container
{

    public class EnterNameUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField _name;
        [SerializeField] TMP_Text _statusText;
        [SerializeField] Button _submit;
        [SerializeField] LoginManager _loginManager;

        public void Back()
        {
            _loginManager.ToLoginSelection();
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