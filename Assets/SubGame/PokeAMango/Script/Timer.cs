using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SuperUltra.JungleDrum;
namespace SuperUltra.JungleDrum
{
    public class Timer : MonoBehaviour
    {

        public float _timeRemaining = 120;
        private bool _timerIsRunning = false;
        [SerializeField]
        private TextMeshProUGUI _timeText;
        [SerializeField]
        private GameObject _end, _fire;
        [SerializeField]
        DeadClicker _deadclicker;
        [SerializeField]
        private GameObject _yesButton;

        private void Start()
        {
            // Starts the timer automatically
            _timerIsRunning = true;
            _deadclicker = _end.GetComponent<DeadClicker>();

        }
        void Update()
        {
            if (_timerIsRunning)
            {
                if (_timeRemaining > 0)
                {
                    _timeRemaining -= Time.deltaTime;
                    DisplayTime(_timeRemaining);
                }
                else
                {
                    _fire.SetActive(false);
                    _timeRemaining = 0;
                    _timerIsRunning = false;
                    _end.SetActive(true);
                    _yesButton.SetActive(false);
                    _deadclicker.GameEnd();
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
    }
}