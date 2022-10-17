using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Animations;
using UnityEngine.UI;
using Lofelt.NiceVibrations;
using SuperUltra.JungleDrum;
using SuperUltra.Container;

namespace SuperUltra.JungleDrum
{
    public class DeadClicker : MonoBehaviour
    {
        [SerializeField]
        GameObject _deadUI;
        private float zvalue = 50;
        AudioSource _endSfx;
        [SerializeField]
        GameObject _gv;
        [SerializeField]
        GameObject _gm;
        [SerializeField]
        GameObject _player;
        GameStat _gs;
        Timer _timer;
        [SerializeField]
        HapticClip _vir;
        [SerializeField]
        Clicker _clicker;
        [SerializeField]
        bool _aniBlocked;
        [SerializeField]
        Scroller _scroller;
        [SerializeField]
        GameObject _ball, _thunder, _thunderBack;
        [SerializeField]
        Transform _ballpt;
        [SerializeField]
        AudioSource _lighting;
        private Shaker _shaker;
        bool _isEnd = false;

        private void Start()
        {
            _endSfx = gameObject.GetComponent<AudioSource>();
            _gs = _gm.GetComponent<GameStat>();
            _timer = _gm.GetComponent<Timer>();
            _clicker = _clicker.GetComponent<Clicker>();
            _shaker = gameObject.GetComponent<Shaker>();
        }

        void Update()
        {
            transform.Rotate(0, 0, zvalue * Time.deltaTime);
            if (_gs._life == 0 && _isEnd == false)
            {
                GameEnd();
                _isEnd = true;
            }

        }
        IEnumerator PauseGame()
        {
            yield return new WaitForSeconds(0.5f);
            Time.timeScale = 0;

        }

        IEnumerator EndGamee()
        {
            yield return new WaitForSeconds(2.5f);
            //_deadUI.SetActive(true);
            ContainerInterface.GameOver();
            EffectControl _ef = _gv.GetComponent<EffectControl>();
            _ef.DofOn();
        }

        public void OnClick()
        {
            if (SceneManager.GetActiveScene().name == "Tourment")
            {
                HapticController.Play(_vir);
                PtReduce();
                if(_gs._life == 0)
                {
                    GameEnd();
                }

            }
            else
            {
                HapticController.Play(_vir);
                GameEnd();

            }
            Invoke("AniUnlock", 3f);
            _aniBlocked = true;
        }

        private void AniUnlock()
        {
            _aniBlocked = false;
        }
        public void GameEnd()
        {
            _player.GetComponent<Animator>().SetTrigger("Dead");
            gameObject.GetComponent<Button>().interactable = false;
            //_endSfx.Play();
            _timer.enabled = false;
            _gs._isSwitching = false;
            _thunder.SetActive(true);
            _thunderBack.SetActive(true);
            _lighting.Play();
            StartCoroutine(Thunder());
            _shaker.Shake();

            
            StartCoroutine(EndGamee());

        }

        public void PtReduce()
        {
            if (!_aniBlocked)
            {
                Instantiate(_ball, _ballpt, true);
                //Invoke("RollAniPlay", 0.05f);
                _thunder.SetActive(true);
                _thunderBack.SetActive(true);
                //_endSfx.Play();
                _lighting.Play();
                StartCoroutine(Thunder());
                _gs.ReduceLife();
                _shaker.Shake();
                if (_gs._score >= 20)
                    _gs._score -= 20;
                else
                    _gs._score = 0;
            }
            _clicker._moveValue = -2f;
            //_player.transform.position = new Vector3(-2, 1.616f, 10f);

        }

        IEnumerator Thunder()
        {

            yield return new WaitForSeconds(0.5f);
            _thunder.SetActive(false);
            _thunderBack.SetActive(false);
        }

    }
}
