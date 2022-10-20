using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using TMPro;

namespace SuperUltra.Container
{

    public class GameListUI : MonoBehaviour
    {
        [SerializeField] RectTransform _gameInfoButtonPrefab;
        [SerializeField] TMP_Text _progressText;
        [SerializeField] Image _progressBar;
        [SerializeField] RectTransform _buttonContainer;
        Dictionary<int, GameInfoUI> _gameIdToInfoMap = new Dictionary<int, GameInfoUI>();

        public void CreateButtons(string gameName, int gameId, float downloadSize, Sprite posterImage, UnityAction callback)
        {
            RectTransform gameInfoButton = Instantiate(_gameInfoButtonPrefab, _buttonContainer);

            GameInfoUI gameInfoUI = gameInfoButton.GetComponent<GameInfoUI>();
            if (gameInfoUI)
            {
                _gameIdToInfoMap.Add(gameId, gameInfoUI);
                gameInfoUI.Initialize(
                    posterImage,
                    downloadSize,
                    callback
                );
            }
        }

        public void UpdateProgress(float percentComplete, int gameId)
        {
            if (!_gameIdToInfoMap.TryGetValue(gameId, out GameInfoUI gameInfoUI))
            {
                return;
            }
            gameInfoUI.SetProgress(percentComplete);
        }

        public void ShowDownloadDisplay(int gameId)
        {
            if (!_gameIdToInfoMap.TryGetValue(gameId, out GameInfoUI gameInfoUI))
            {
                return;
            }
            gameInfoUI.ShowDownloadProgress();
        }

        public void SetDownloadIconVisible(int gameId, bool isDownloaded)
        {
            if (!_gameIdToInfoMap.TryGetValue(gameId, out GameInfoUI gameInfoUI))
            {
                return;
            }
            gameInfoUI.SetIconVisible(isDownloaded);
        }

        public void SetButtonCallback(int gameId, UnityAction callback)
        {
            if (!_gameIdToInfoMap.TryGetValue(gameId, out GameInfoUI gameInfoUI))
            {
                return;
            }
            gameInfoUI.SetButtonCallback(callback);
        }

    }

}

