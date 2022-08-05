using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace SuperUltra.Container
{

    public class MessagePopUpUI : MonoBehaviour
    {
        [SerializeField] PopUpUI _popUpUI;
        [SerializeField] Button _actionButton;
        [SerializeField] Button _closeButton;


        public void Show(
            string message = "",
            string actionButtonText = "",
            Action actionButtonCallback = null,
            bool shouldHideAfterAction = false,
            bool shouldShowCloseButton = true,
            Action closeButtonCallback = null
        )
        {
            gameObject.SetActive(true);
            TMP_Text text = _actionButton.GetComponentInChildren<TMP_Text>();

            SetActionButtonListener(actionButtonCallback, shouldHideAfterAction);
            _actionButton.gameObject.SetActive(!string.IsNullOrEmpty(actionButtonText));
            if (text)
            {
                text.text = actionButtonText;
            }

            SetCloseButtonListener(closeButtonCallback);
            _closeButton.gameObject.SetActive(shouldShowCloseButton);

            _popUpUI.Show(message);
        }

        void Hide()
        {
            _actionButton.onClick.RemoveAllListeners();
            _popUpUI.Hide().OnComplete(() => gameObject.SetActive(false));
        }

        void SetCloseButtonListener(Action closeButtonCallback = null)
        {
            _closeButton.onClick.RemoveAllListeners();
            _closeButton.onClick.AddListener(() =>
            {
                if(closeButtonCallback != null)
                {
                    closeButtonCallback();
                }
                Hide();
            });
        }

        void SetActionButtonListener(Action actionButtonCallback = null, bool shouldHideAfterAction = false)
        {
            _actionButton.onClick.RemoveAllListeners();
            _actionButton.onClick.AddListener(() =>
            {
                if (actionButtonCallback != null)
                {
                    actionButtonCallback();
                }
                if (shouldHideAfterAction)
                {
                    Hide();
                }
            });
        }
    }

}