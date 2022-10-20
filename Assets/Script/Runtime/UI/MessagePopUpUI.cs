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
        [SerializeField] TMP_Text _messageText;
        static MessagePopUpUI _instance;
        public static MessagePopUpUI instance
        {
            get { return _instance; }
            private set { _instance = value; }
        }

        void Awake()
        {
            if (_instance != null)
            {
                Destroy(this.gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        public void Show(
            string message = "",
            string actionButtonText = "",
            Action actionButtonCallback = null,
            bool shouldHideAfterAction = true,
            bool shouldShowCloseButton = true,
            Action closeButtonCallback = null
        )
        {
            gameObject.SetActive(true);

            SetActionButtonText(actionButtonText, actionButtonCallback, shouldHideAfterAction);
            SetMessage(message);
            SetCloseButtonListener(closeButtonCallback);

            _closeButton.gameObject.SetActive(shouldShowCloseButton);

            _popUpUI.Show(message).SetUpdate(true);
        }

        public void ShowCustomContent(
            RectTransform content,
            string actionButtonText = "",
            Action actionButtonCallback = null,
            bool shouldHideAfterAction = false
        )
        {
            gameObject.SetActive(true);
            if (_contentContainer)
            {
                Instantiate(content, _contentContainer);
            }
            SetActionButtonListener(actionButtonCallback, shouldHideAfterAction);
            _actionButton.gameObject.SetActive(!string.IsNullOrEmpty(actionButtonText));
            TMP_Text text = _actionButton.GetComponentInChildren<TMP_Text>();
            if (text)
            {
                text.text = actionButtonText;
            }
            _popUpUI.Show().SetUpdate(true);
        }

        public void Hide()
        {
            if (_contentContainer)
            {
                foreach (Transform item in _contentContainer)
                {
                    Destroy(item.gameObject);
                }
            }
            if (_messageText)
            {
                _messageText.text = "";
            }
            _actionButton.onClick.RemoveAllListeners();
            _popUpUI.Hide().SetUpdate(true).OnComplete(() => gameObject.SetActive(false));
        }

        void SetActionButtonText(string message, Action callback, bool shouldHideAfterAction)
        {
            TMP_Text text = _actionButton.GetComponentInChildren<TMP_Text>();

            SetActionButtonListener(callback, shouldHideAfterAction);
            _actionButton.gameObject.SetActive(!string.IsNullOrEmpty(message));
            if (text)
            {
                text.text = message;
            }
        }

        void SetMessage(string message)
        {
            if (_messageText)
            {
                _messageText.text = message;
            }
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

        void SetActionButtonListener(Action actionButtonCallback = null, bool shouldHideAfterAction = true)
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

        public static void Show(
            string message = "",
            string actionButtonText = "",
            Action actionButtonCallback = null,
            bool shouldHideAfterAction = true
        )
        {
            if (_instance)
            {
                _instance.Show(
                    message,
                    actionButtonText,
                    actionButtonCallback,
                    shouldHideAfterAction
                );
            }
        }

        public static void Show(
            RectTransform content,
            string actionButtonText = "",
            Action actionButtonCallback = null,
            bool shouldHideAfterAction = false
        )
        {
            if (_instance)
            {
                _instance.ShowCustomContent(
                    content,
                    actionButtonText,
                    actionButtonCallback,
                    shouldHideAfterAction
                );
            }
        }
    }

}