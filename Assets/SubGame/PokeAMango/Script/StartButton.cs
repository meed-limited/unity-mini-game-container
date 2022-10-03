using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using SuperUltra.JungleDrum;

namespace SuperUltra.JungleDrum
{
    public class StartButton : MonoBehaviour
    {
        [SerializeField]
        GameObject _gv;
        [SerializeField]
        GameObject _clicker;
        [SerializeField]
        StiLog _stiTimer;

        private void Start()
        {
            _stiTimer = _stiTimer.GetComponent<StiLog>();
        }
        public void OnClick()
        {
            _stiTimer.UseSti();
            _clicker.GetComponent<Clicker>().StartRotate();
            Time.timeScale = 1;
            EffectControl _ef = _gv.GetComponent<EffectControl>();
            _ef.DofOff();
            gameObject.SetActive(false);
        }
    }
}
