using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.JungleDrum;
using SuperUltra.Container;

namespace SuperUltra.JungleDrum {

    public class PauseButton : MonoBehaviour
    {
        [SerializeField]
        GameObject _pauseWindow;
        [SerializeField]
        GameObject _gv;
        private void OnEnable()
        {
            ContainerInterface.OnPauseMenuHide += ResumeGame;
        }
        private void OnDisable()
        {
            ContainerInterface.OnPauseMenuHide -= ResumeGame;
        }
        public void PauseGame()
        {
            ContainerInterface.Pause();
            EffectControl _ef = _gv.GetComponent<EffectControl>();
            //_ef.DofOn();
            Time.timeScale = 0;
            //_pauseWindow.SetActive(true);
        }

        public void ResumeGame()
        {
            //ContainerInterface.Resume();
            EffectControl _ef = _gv.GetComponent<EffectControl>();
            _ef.DofOff();
            Time.timeScale = 1;
            //_pauseWindow.SetActive(false);
        }
    }
}
