using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Animations;
using SuperUltra.JungleDrum;

namespace SuperUltra.JungleDrum
{

    public class GameStat : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI scoreText;

        public int _score = 0;
        [SerializeField]
        private float _switchTimer;
        [SerializeField]
        private GameObject _yesButton;
        [SerializeField]
        private GameObject _noButton, _fire;
        [SerializeField]
        private TextMeshProUGUI _timerText;
        [SerializeField]
        private TextMeshProUGUI _bestText;
        [SerializeField]
        private TextMeshProUGUI _endBestText;
        [SerializeField]
        private TextMeshProUGUI _endScoreText;
        public bool _isSwitching = true;
        [SerializeField]
        Animator _running;
        [SerializeField]
        private GameObject _speed;

        private void Awake()
        {
            _bestText.text = PlayerPrefs.GetInt("Best").ToString();

        }
        private void Start()
        {
            _switchTimer = Random.Range(2f, 10f);
        }

        private void Update()
        {

            scoreText.text = _score.ToString();
            _endScoreText.text = _score.ToString();
            _endBestText.text = PlayerPrefs.GetInt("Best").ToString();
            _switchTimer -= Time.deltaTime;
            _timerText.text = _switchTimer.ToString();
            if (_isSwitching)
                if (_switchTimer <= 0)
                {
                    if (_yesButton.activeSelf)
                    {
                        _yesButton.SetActive(false);
                        _noButton.SetActive(true);
                        _switchTimer = Random.Range(1.5f, 3f);
                        _running.SetBool("runningtri", false);
                        _fire.SetActive(false);
                        //_speed.SetActive(false);
                    }
                    else
                    {
                        _yesButton.SetActive(true);
                        _noButton.SetActive(false);
                        _switchTimer = Random.Range(2f, 6f);
                        _fire.SetActive(true);
                    }



                    //function
                }

        }
        public void GetSocre()
        {
            _score += 1;
            if (_score > PlayerPrefs.GetInt("Best"))
                PlayerPrefs.SetInt("Best", _score);
        }




    }
}
