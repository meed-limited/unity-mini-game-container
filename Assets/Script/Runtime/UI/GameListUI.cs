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

        public void CreateButtons(string key, float downloadSize, UnityAction callback)
        {
            RectTransform gameInfoButton = Instantiate(_gameInfoButtonPrefab, _buttonContainer);
            Button button = gameInfoButton.GetComponentInChildren<Button>();
            TMP_Text text = gameInfoButton.GetComponentsInChildren<TMP_Text>()[1];
            button.GetComponentInChildren<TMP_Text>().text = key;
            button.onClick.AddListener(callback);
            text.text = $"Download size: {downloadSize} bytes";
        }        

        public void UpdateProgress(float percentComplete, string taskName)
        {
            _progressBar.fillAmount = percentComplete;
            _progressText.text = taskName;
        }

        public void UpdateResult(string taskName, bool result)
        {
            _progressBar.fillAmount = 1;
            _progressText.text = $"{taskName} {(result ? "Success" : "Failed")}";
        }

        public void ToProfile()
        {
            SceneLoader.ToProfile();
        }
        
    }
    
}

