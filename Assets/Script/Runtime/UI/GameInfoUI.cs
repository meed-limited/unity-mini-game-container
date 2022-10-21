using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using TMPro;

namespace SuperUltra.Container
{

    public class GameInfoUI : MonoBehaviour
    {
        [SerializeField] Image _progress;
        [SerializeField] Image _downloadIcon;
        [SerializeField] TMP_Text _downloadSizeText;
        [SerializeField] Image _poster;
        [SerializeField] Button _button;
        [SerializeField] RectTransform _downloadPanel;

        public void Initialize(Sprite posterImage, float downloadSize, UnityAction callback)
        {
            SetIconVisible(downloadSize <= 1);
            SetImage(posterImage);
            SetButtonCallback(callback);
            SetDownloadSize(downloadSize);
        }

        public void SetButtonCallback(UnityAction callback)
        {
            if (_button)
            {
                _button.enabled = true;
                _button.onClick.AddListener(
                    () =>
                    {
                        callback?.Invoke();
                        _button.enabled = false;
                    }
                );
            }
        }

        public void SetIconVisible(bool isDownloaded)
        {
            if (isDownloaded)
            {
                _downloadPanel.gameObject.SetActive(false);
                return;
            }
            _downloadPanel.gameObject.SetActive(true);
            _downloadIcon.gameObject.SetActive(true);
            _progress.gameObject.SetActive(false);
        }

        public void ShowDownloadProgress()
        {
            _downloadPanel.gameObject.SetActive(true);
            _downloadIcon.gameObject.SetActive(false);
            _progress.gameObject.SetActive(true);
        }

        void SetImage(Sprite posterImage)
        {
            if (_poster == null || posterImage == null)
            {
                return;
            }
            _poster.sprite = posterImage;
        }

        void SetDownloadSize(float downloadSize)
        {
            float size = downloadSize / (1000f * 1000f);
            if (_downloadSizeText)
            {
                _downloadSizeText.text = $"{size.ToString("n2")}MB";
            }
        }

        public void SetProgress(float percentComplete)
        {
            if (_progress)
            {
                _progress.fillAmount = percentComplete;
            }
            if (_downloadSizeText)
            {
                _downloadSizeText.text = $"{(percentComplete * 100).ToString("n2")}%\n";
            }
        }

    }

}
