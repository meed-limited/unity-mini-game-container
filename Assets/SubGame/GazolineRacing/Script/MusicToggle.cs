using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.Container;

namespace SuperUltra.GazolineRacing
{

    public class MusicToggle : MonoBehaviour
    {
        [SerializeField] AudioSource _coin, _background, _driving, _explo;

        private void OnEnable()
        {
            ContainerInterface.OnMusicVolumeChange += MusicToggles;
            ContainerInterface.OnEffectVolumeChange += SFXToggle;
            ContainerInterface.OnPauseMenuHide += Resume;
        }

        private void OnDisable()
        {
            ContainerInterface.OnMusicVolumeChange += MusicToggles;
            ContainerInterface.OnEffectVolumeChange += SFXToggle;
            ContainerInterface.OnPauseMenuHide -= Resume;
        }
        
        private void Resume()
        {
            Time.timeScale = 1;
        }
        private void MusicToggles(bool isOn)
        {
            if (isOn)
            {
                _background.enabled = true;
            }
            else
                _background.enabled = false;
        }

        private void SFXToggle(bool isOn)
        {
            if (isOn)
            {
                _coin.enabled = true;
                _driving.enabled = true;
                _explo.enabled = true;
            }
            else
            {
                _coin.enabled = false;
                _driving.enabled = false;
                _explo.enabled = false;
            }
        }
    }
}