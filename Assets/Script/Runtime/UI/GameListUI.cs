using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace SuperUltra.Container
{

    public class GameListUI : MonoBehaviour
    {
        [SerializeField] RectTransform _gameInfoButtonPrefab;
        [SerializeField] TMP_Text _progressText;
        [SerializeField] Image _progressBar;
        [SerializeField] RectTransform _buttonContainer;

        public void CreateButtons(string key, int gameId, float downloadSize, Sprite posterImage, UnityAction callback)
        {
            RectTransform gameInfoButton = Instantiate(_gameInfoButtonPrefab, _buttonContainer);
            Button button = gameInfoButton.GetComponentInChildren<Button>();
            TMP_Text text = gameInfoButton.GetComponentsInChildren<TMP_Text>()[1];
            SetImage(gameInfoButton.GetComponentInChildren<Image>(), posterImage);
            button.GetComponentInChildren<TMP_Text>().text = key;
            button.onClick.AddListener(callback);
            text.text = $"Download size: {downloadSize} bytes";
        }

        void SetImage(Image image, Sprite posterImage)
        {
            if (image == null || posterImage == null)
            {
                return;
            }
            image.sprite = posterImage;
        }

        public void UpdateProgress(float percentComplete, string taskName)
        {
            if (_progressBar)
            {
                _progressBar.fillAmount = percentComplete;
            }
            if (_progressText)
            {
                _progressText.text = taskName;
            }
        }

        public void UpdateResult(string taskName, bool result)
        {
            if (_progressBar)
            {
                _progressBar.fillAmount = 1;
            }
            if (_progressText)
            {
                _progressText.text = $"{taskName} {(result ? "Success" : "Failed")}";
            }
        }

    }

}

