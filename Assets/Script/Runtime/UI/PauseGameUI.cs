using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SuperUltra.Container
{
    public class PauseGameUI : MonoBehaviour
    {

        [SerializeField] Toggle _musicToggle;
        [SerializeField] Toggle _soundToggle;
        [SerializeField] Canvas _pauseMenuCanvas;
        [SerializeField] PopUpUI _popUp;
        static PauseGameUI _instance;

        void Awake()
        {
            if (_instance)
            {
                Destroy(this.gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            ContainerInterface.OnPauseMenuShow += Show;
            ContainerInterface.OnPauseMenuHide += Hide;
            ContainerInterface.OnReturnMenu += Hide;
            Hide();
        }

        void OnDestory()
        {
            ContainerInterface.OnPauseMenuShow -= Show;
            ContainerInterface.OnPauseMenuHide -= Hide;
            ContainerInterface.OnReturnMenu -= Hide;
        }

        void Start()
        {
            if (PlayerPrefs.HasKey(Config.KEY_MUSIC))
            {
                int value = PlayerPrefs.GetInt(Config.KEY_MUSIC);
                SessionData.IsMusicOn = value == 1;
            }
            if (PlayerPrefs.HasKey(Config.KEY_SOUND))
            {
                int value = PlayerPrefs.GetInt(Config.KEY_SOUND);
                SessionData.IsEffectSoundOn = value == 1;
            }
            _musicToggle.isOn = SessionData.IsMusicOn;
            _soundToggle.isOn = SessionData.IsEffectSoundOn;
        }

        public void OnResume()
        {
            ContainerInterface.Resume();
        }

        void Show()
        {
            _pauseMenuCanvas.gameObject.SetActive(true);
            _popUp.Show().SetUpdate(true);
        }

        void Hide()
        {
            _popUp.Hide().SetUpdate(true).OnComplete(
                () =>
                {
                    _pauseMenuCanvas.gameObject.SetActive(false);
                }
            );
        }

        public void SubmitScore()
        {
            LoadingUI.ShowInstance();
            NetworkManager.UpdateScore(
                SessionData.currnetGameScore,
                UserData.playFabId,
                SessionData.currentGameId,
                OnUpdateScore
            );
        }

        void OnUpdateScore(UpdateScoreResponseData data)
        {
            LoadingUI.HideInstance();
            Hide();
            if (!data.result)
            {
                MessagePopUpUI.Show(
                    data.message,
                    "Back to Menu",
                    ContainerInterface.ReturnToMenu
                );
                return;
            }
            ContainerInterface.ReturnToMenu();
        }

        public void HowToPlay()
        {
            ContainerInterface.HowToPlay();
        }

        public void ToggleMusic(bool isOn)
        {
            SessionData.IsMusicOn = isOn;
            ContainerInterface.MusicVolumeChange(isOn);
            PlayerPrefs.SetInt(Config.KEY_MUSIC, isOn ? 1 : 0);
        }

        public void ToggleSoundEffect(bool isOn)
        {
            SessionData.IsEffectSoundOn = isOn;
            ContainerInterface.EffectVolumeChange(isOn);
            PlayerPrefs.SetInt(Config.KEY_SOUND, isOn ? 1 : 0);
        }

    }
}