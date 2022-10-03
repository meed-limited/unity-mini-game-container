using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using SuperUltra.GazolineRacing;
using SuperUltra.Container;

namespace SuperUltra.GazolineRacing
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _scoreText;
        [SerializeField]
        private TMPro.TextMeshProUGUI _lifeText;
        [SerializeField]
        private TMPro.TextMeshProUGUI _finalScoreText;
        [SerializeField]
        private GameObject _endMenu;
        private float _score;
        private int _timeMulti = 2;
        private int _life = 3;
        private int _bananaCount = 0;
        [SerializeField] Slider _bananaBar;
        [SerializeField] GameObject[] _lifeBar;
        [SerializeField] GameObject _endSfx, _engineSfx;

        private void Awake()
        {
            for (int i = 0; i < 3; i++)
            {
                _lifeBar[i].SetActive(true);
            }
        }
        private void Update()
        {

            _score += _timeMulti * Time.deltaTime;
            _scoreText.text = _score.ToString("F0");
            //Debug.Log(_score);
            if (_life == 0)
            {
                Invoke("GameEnd", 0.3f);
            }
        }

        public void GetScore(int score)
        {
            _score += score;
            _bananaCount += 1;
            _bananaBar.value = _bananaCount;
            if (_bananaCount == _bananaBar.maxValue)
            {
                _bananaCount = 0;
                _bananaBar.value = _bananaCount;
            }
        }
        public void RequestLifeAds()
        {
            ContainerInterface.RequestRewardedAds(
                RequestLifeRewardedAdCallback
            );
        }

        void RequestLifeRewardedAdCallback(bool result)
        {
            Debug.Log("RequestLifeRewardedAdCallback " + result);
            if (result)
            {
                AddLife();
            }
        }
        public void AddLife()
        {
            if (_life == 3)
            {
                for (int i = 0; i < 5; i++)
                {
                    _life += 1;
                    _lifeBar[_life].SetActive(true);

                }

            }
            else if (_life == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    _life += 1;
                    _lifeBar[_life].SetActive(true);

                }
            }


        }
        public void ReduceLife()
        {
            if (_life > 0)
                _life -= 1;

            else
            {
                _life = 0;
            }
            _lifeBar[_life].GetComponent<RectTransform>().DOLocalMoveY(-21f, 0.2f, false).OnComplete(() => _lifeBar[_life].SetActive(false));
            //_lifeBar[_life].SetActive(false);
        }

        public void GameEnd()
        {
            _engineSfx.SetActive(false);
            _endSfx.SetActive(true);
            ContainerInterface.GameOver();
            ContainerInterface.SetScore(Mathf.Round(_score));
            //_endMenu.SetActive(true);
            _finalScoreText.text = _score.ToString("F0");
            StartCoroutine(GameStop());
        }

        IEnumerator GameStop()
        {
            yield return new WaitForSeconds(0.5f);
            Time.timeScale = 0;
        }
    }
}