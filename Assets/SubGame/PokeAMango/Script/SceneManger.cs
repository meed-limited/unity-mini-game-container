using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SuperUltra.Container;
using SuperUltra.JungleDrum;

namespace SuperUltra.JungleDrum
{

    public class SceneManger : MonoBehaviour
    {
        [SerializeField]
        private GameObject _tutor;
        [SerializeField]
        private EffectControl _ev;
        [SerializeField]
        private GameObject _start;


        private void Awake()
        {
            Debug.Log("awake");
            Time.timeScale = 0;
        }

        private void Start()
        {
            if (!PlayerPrefs.HasKey("firstTime")) //return true if the key exist
            {
                _ev.DofOff();
                _tutor.SetActive(true);
                _start.SetActive(false);
                Time.timeScale = 1;
                PlayerPrefs.SetInt("firstTime", 0);
            }
            else
            {

                print("It is not the first time in the game.");
                //because the key "firstTime"
            }
        }
        public void RestartSenece()
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }

        public void Tourment()
        {
            SceneManager.LoadScene(2);
        }

        public void SuddenDead()
        {
            SceneManager.LoadScene(1);
        }

    }
}


