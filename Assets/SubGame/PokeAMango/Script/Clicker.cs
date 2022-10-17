using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lofelt.NiceVibrations;
using UnityEngine.Animations;
using DG.Tweening;
using System;
using UnityEngine.UI;
using SuperUltra.JungleDrum;

namespace SuperUltra.JungleDrum
{
    public class Clicker : MonoBehaviour
    {
        private float zvalue = 50;
        public GameObject gameman;
        public GameObject m_camera;
        Animator _anim;
        [SerializeField]
        private float _colorValueX = 1;
        [SerializeField]
        private float _colorValueY = 0.99f;
        [SerializeField]
        private float _colorValueZ = 0.7f;
        [SerializeField]
        private AudioSource _sfx;
        [SerializeField]
        ParticleSystem _clickeffect, _dustEffect, _sDust;

        bool _rotating = true; ///change before launch

        GameObject _runner;
        [SerializeField]
        GameObject _speedLine;
        [SerializeField]
        Animator _runnerAni;
        [SerializeField]
        private float _stopCount = 0.3f;

        public float _moveValue = -2f;
        [SerializeField]
        Scroller _scroll;
        float lastTime;
        float bpm = 0;
        [SerializeField]
        Sprite[] _faceSprites;
        [SerializeField]
        Sprite[] _eyesSprites;
        [SerializeField]
        GameObject _rawImage;
        [SerializeField]
        GameObject _dustPoint, _fire;
        [SerializeField] FireFXPool _firePooler;







        private void Start()
        {
            _anim = gameObject.GetComponent<Animator>();
            _sfx = gameObject.GetComponent<AudioSource>();
            _runner = GameObject.FindGameObjectWithTag("Player");
            _runnerAni = _runner.GetComponent<Animator>();

        }

        void Update()
        {
            RotateObject();

        }

        private void ChangeFace()
        {
            if (bpm >= 150 && bpm < 250)
            {
                gameObject.GetComponent<Image>().sprite = _faceSprites[1];
                _rawImage.GetComponent<Image>().sprite = _eyesSprites[1];
            }
            else if (bpm >= 250 && bpm < 350)
            {
                gameObject.GetComponent<Image>().sprite = _faceSprites[2];
                _rawImage.GetComponent<Image>().sprite = _eyesSprites[2];
            }
            else if (bpm >= 350 && bpm < 400)
            {
                gameObject.GetComponent<Image>().sprite = _faceSprites[3];
                _rawImage.GetComponent<Image>().sprite = _eyesSprites[3];
            }
            else if (bpm >= 400)
            {
                gameObject.GetComponent<Image>().sprite = _faceSprites[4];
                _rawImage.GetComponent<Image>().sprite = _eyesSprites[4];
            }
            else
            {
                gameObject.GetComponent<Image>().sprite = _faceSprites[0];
                _rawImage.GetComponent<Image>().sprite = _eyesSprites[0];
            }

        }

        private void BpmTrack()
        {

            float currentTime = Time.time;
            float diff = currentTime - lastTime;
            lastTime = currentTime;
            bpm = 60f / diff;
            _runnerAni.SetFloat("Speed", bpm);
            //Debug.Log(bpm);

        }

        private void OnEnable()
        {
            //_anim.Play();
        }


        public void OnClick()
        {
            gameman.GetComponent<GameStat>().GetSocre();
            m_camera.GetComponent<Camera>().backgroundColor = new Color(_colorValueX, _colorValueY, _colorValueZ);

            RunningAni();
            ColorChange();
            EffectPlay();
            _scroll.MountMove();
            _anim.SetTrigger("AniPlayer");
            SpwanCoin();

            //Debug.Log("Clicked");

            if (_sfx != null)
            {
                _sfx.Play();
            }
            BpmTrack();

        }

        private void SpwanCoin()
        {

            //var _coin = Instantiate(_clickeffect, new Vector3(0f, -2.6f, 1.7f), Quaternion.identity);
            _firePooler.Spawn();
        }

        private void EffectPlay()
        {
            if (bpm < 280)
            {
                _fire.SetActive(true);
                _fire.GetComponent<ParticleSystem>().Play(true);
                _fire.transform.localScale = new Vector3(1, 1, 1);
                //_speedLine.SetActive(false);
            }
            else if (bpm >= 280)
            {
                _fire.SetActive(true);
                _fire.transform.localScale = new Vector3(4, 4, 4);
                //_speedLine.SetActive(true);
            }
            else
            {
                _fire.SetActive(false);
                //_speedLine.SetActive(false);
            }
        }

        private void ColorChange()
        {
            if (_colorValueX >= 1 && _colorValueY < 1 && _colorValueZ < 1)
            {
                //Debug.Log("C1");
                _colorValueY -= 0.02f;
                _colorValueZ += 0.02f;
            }
            else if (_colorValueX <= 0.45 && _colorValueY < 1 && _colorValueZ >= 1)
            {
                _colorValueY += 0.02f;
                //Debug.Log("C2");
            }
            else if (_colorValueX >= 1 && _colorValueY < .8 && _colorValueZ > .8)
            {
                _colorValueZ = 0.8f;
                _colorValueX -= 0.02f;

                //Debug.Log("C3");
            }
            else
            {
                _colorValueX -= 0.02f;
                //Debug.Log("C4");

            }
        }

        public void StartRotate()
        {
            _rotating = true;
        }

        private void RunningAni()
        {
            _runnerAni.SetBool("runningtri", true);
            _runner.transform.DOMoveX(_moveValue, 0.2f, false);

            //_runner.transform.position += new Vector3(_moveValue, 0, 0);
            _stopCount = 0.3f;
            _moveValue += 0.05f;

            if (_moveValue >= 1.6)
            {
                _moveValue = -2f;
            }
        }

        private void RotateObject()
        {
            if (_rotating)
                transform.Rotate(0, 0, zvalue * Time.deltaTime);
            _stopCount -= Time.deltaTime;
            if (_stopCount < 0)
            {
                _runnerAni.SetBool("runningtri", false);
                _stopCount = 0;
            }
        }
    }
}
