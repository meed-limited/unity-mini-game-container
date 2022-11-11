using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SuperUltra.GazolineRacing;
using SuperUltra.Container;

namespace SuperUltra.GazolineRacing
{

    public class Timer : MonoBehaviour
    {

        public float _timeRemaining = 120;
        private bool _timerIsRunning = false;
        [SerializeField]
        private TextMeshProUGUI _timeText;
        //[SerializeField]
        //private GameObject _end, _fire;
        [SerializeField]
        GameManager _gm;


        private void Start()
        {
            // Starts the timer automatically
            _timerIsRunning = true;

        }
        void Update()
        {

            if (_gm.isEnd == true)
                _timerIsRunning = false;

            if (_timerIsRunning)
            {
                if (_timeRemaining > 0)
                {
                    _timeRemaining -= Time.deltaTime;
                    DisplayTime(_timeRemaining);
                }
                else
                {
                    //_fire.SetActive(false);
                    _timeRemaining = 0;
                    _timerIsRunning = false;
                    _gm.GameEnd();
                    //_end.SetActive(true);

                }
            }
        }
        void DisplayTime(float timeToDisplay)
        {
            timeToDisplay += 1;
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            _timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        public void RequestTimeAds()
        {
            ContainerInterface.RequestRewardedAds(
                RequestTimeRewardedAdCallback
            );
        }

        void RequestTimeRewardedAdCallback(bool result)
        {
            Debug.Log("RequestLifeRewardedAdCallback " + result);
            if (result)
            {
                _timeRemaining += 30f;
            }
        }
    }

    

}