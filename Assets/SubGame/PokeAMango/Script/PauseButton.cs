using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.JungleDrum;

namespace SuperUltra.JungleDrum {

    public class PauseButton : MonoBehaviour
    {
        [SerializeField]
        GameObject _pauseWindow;
        [SerializeField]
        GameObject _gv;

        public void PauseGame()
        {
            EffectControl _ef = _gv.GetComponent<EffectControl>();
            _ef.DofOn();
            Time.timeScale = 0;
            _pauseWindow.SetActive(true);
        }

        public void ResumeGame()
        {
            EffectControl _ef = _gv.GetComponent<EffectControl>();
            _ef.DofOff();
            Time.timeScale = 1;
            _pauseWindow.SetActive(false);
        }
    }
}
