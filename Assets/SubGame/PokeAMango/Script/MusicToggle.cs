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
        }

        private void OnDisable()
        {
            ContainerInterface.OnMusicVolumeChange -= MusicOnOff;
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