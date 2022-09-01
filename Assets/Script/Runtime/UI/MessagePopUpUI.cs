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
        [SerializeField] RectTransform _contentContainer;

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

        public void Show(
            RectTransform content,
            string actionButtonText = "",
            Action actionButtonCallback = null,
            bool shouldHideAfterAction = false
        )
        {
            Instantiate(content, _contentContainer);
            SetActionButtonListener(actionButtonCallback, shouldHideAfterAction);
            _actionButton.gameObject.SetActive(!string.IsNullOrEmpty(actionButtonText));
            TMP_Text text = _actionButton.GetComponentInChildren<TMP_Text>();
            if (text)
            {
                text.text = actionButtonText;
            }
            _popUpUI.Show();
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
                if (closeButtonCallback != null)
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
                foreach (Transform item in _contentContainer)
                {
                    Destroy(item.gameObject);
                }
            });
        }
    }

}