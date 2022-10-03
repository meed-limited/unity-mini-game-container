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


        private void OnEnable()
        {
            ContainerInterface.OnMusicVolumeChange += MusicOnOff;
            ContainerInterface.OnPauseMenuHide += Resume;
        }

        private void OnDisable()
        {
            ContainerInterface.OnMusicVolumeChange -= MusicOnOff;
            ContainerInterface.OnPauseMenuHide -= Resume;
        }

        private void Resume()
        {
            Time.timeScale = 1;
        }
        public void MusicOnOff(bool isOn)
        {
            if (isOn)
            {
                _music.SetActive(true);
                //ContainerInterface.MusicVolumeChange(true);
                Debug.Log("IsOn");
            }
            else
            {
                _music.SetActive(false);
                //ContainerInterface.MusicVolumeChange(false);
                Debug.Log("IsOn");  

            }
        }
    }
}