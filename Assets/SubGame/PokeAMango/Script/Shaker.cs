using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SuperUltra.JungleDrum;

namespace SuperUltra.JungleDrum {

    public class Shaker : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _textUI;
        [SerializeField]
        private Camera _camera;
        [SerializeField]
        private float _duration = 0.5f;
        private float _strength = 30f;
        private int _vibrato = 80;
        private float _randomness = 10f;


        public void Shake()
        {
            _textUI.DOShakeAnchorPos(_duration, _strength, _vibrato, _randomness, true, true).OnComplete(() =>
            {
                _textUI.DORewind(true);
            });


            _camera.DOShakePosition(_duration, 1, 8, 50, true).OnComplete(() =>
            {
                _camera.DORewind(true);
            });

        }



    }
}
