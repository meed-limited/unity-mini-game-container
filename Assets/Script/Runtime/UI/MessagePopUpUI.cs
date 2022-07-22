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

        public void Show(
            string message = "",
            string actionButtonText = "",
            Action actionButtonCallback = null,
            bool shouldHideAfterAction = false
        )
        {
            gameObject.SetActive(true);
            TMP_Text text = _actionButton.GetComponentInChildren<TMP_Text>();
            SetActionButtonListener(actionButtonCallback, shouldHideAfterAction);
            if (text)
            {
                text.text = actionButtonText;
            }
            _popUpUI.Show(message);
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
                    _popUpUI.Hide().OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                    });
                }
            });
        }
    }

}