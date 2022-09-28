using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SuperUltra.Container;

namespace SuperUltra.JungleDrum
{
    public class MusicToggle : MonoBehaviour
    {
        [SerializeField]
        GameObject _music;
        [SerializeField]
        private Toggle _toggle;

        private void OnEnable()
        {
            ContainerInterface.OnMusicVolumeChange += MusicOnOff;
        }

        private void OnDisable()
        {
            ContainerInterface.OnMusicVolumeChange -= MusicOnOff;
        }

        private void Start()
        {
            _toggle = gameObject.GetComponent<Toggle>();
        }
        public void MusicOnOff(bool isOn)
        {
            if (isOn)
            {
                _music.SetActive(true);
                ContainerInterface.MusicVolumeChange(true);
            }
            else
                _music.SetActive(false);
                ContainerInterface.MusicVolumeChange(false);
        }
    }
}