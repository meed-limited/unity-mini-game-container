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
            Hide();
        }

        void OnDestory()
        {
            ContainerInterface.OnPauseMenuShow -= Show;
            ContainerInterface.OnPauseMenuHide -= Hide;
        }

        void Start()
        {
            _musicToggle.isOn = Setting.IsMusicOn;
            _soundToggle.isOn = Setting.IsEffectSoundOn;
        }

        public void OnResume()
        {
            ContainerInterface.Resume();
        }

        void Show()
        {
            _pauseMenuCanvas.gameObject.SetActive(true);
            _popUp.Show();
        }

        void Hide()
        {
            _popUp.Hide().OnComplete(
                () =>
                {
                    _pauseMenuCanvas.gameObject.SetActive(false);
                }
            );
        }

        public void SubmitScore()
        {
            LoadingUI.Show();
            NetworkManager.UpdateScore(
                100, //TODO
                UserData.playFabId,
                GameData.currentGameId,
                () =>
                {
                    LoadingUI.Hide();
                    ContainerInterface.ReturnToMenu();
                }
            );
        }

        public void HowToPlay()
        {
            ContainerInterface.HowToPlay();
        }

        public void tesT()
        {
            ContainerInterface.GameOver();
        }

        public void ToggleMusic(bool isOn)
        {
            Setting.IsMusicOn = isOn;
            ContainerInterface.MusicVolumeChange(isOn);
        }

        public void ToggleSoundEffect(bool isOn)
        {
            Setting.IsEffectSoundOn = isOn;
            ContainerInterface.EffectVolumeChange(isOn);
        }

    }
}