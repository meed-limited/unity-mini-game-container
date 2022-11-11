using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SuperUltra.GazolineRacing;
using SuperUltra.Container;

namespace SuperUltra.GazolineRacing
{


    public class SceneManger : MonoBehaviour
    {
        [SerializeField]
        GameObject _pause, _start;
        [SerializeField]
        AudioSource _carStart, _music;

        private void Awake()
        {
            //Debug.Log("awake");
            Time.timeScale = 0;
        }

        public void RestartSenece()
        {
            ContainerInterface.PlayAgain();
            //SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }
        public void StartGame()
        {
            _start.SetActive(false);
            _carStart.enabled = true;
            Time.timeScale = 1;
            StartCoroutine(StartMusic());
        }
        public void OpenPause()
        {
            //_pause.SetActive(true);
            ContainerInterface.Pause();
            Time.timeScale = 0;
        }
        public void ClosePause()
        {
            _pause.SetActive(false);
            Time.timeScale = 1;
        }
        IEnumerator StartMusic()
        {
            yield return new WaitForSeconds(0.7f);
            _music.enabled = true;
        }
    }


}