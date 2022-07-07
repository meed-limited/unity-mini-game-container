using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Shaker : MonoBehaviour
{
    [SerializeField]
    private RectTransform _scoreText, _bestText, _fullTimer, _pauseButton, _noButton, _deadUI;
    [SerializeField]
    private float _duration = 0.5f;
    private float _strength = 30f;
    private int _vibrato = 10;
    private float _randomness = 20f;
    public void Shake()
    {
        _scoreText.DOShakeAnchorPos(_duration, _strength, _vibrato, _randomness, false, false);
        _bestText.DOShakeAnchorPos(_duration, _strength, _vibrato, _randomness, false, false);
        _fullTimer.DOShakeAnchorPos(_duration, _strength, _vibrato, _randomness, false, false);
        _pauseButton.DOShakeAnchorPos(_duration, _strength, _vibrato, _randomness, false, false);
        _noButton.DOShakeAnchorPos(_duration, _strength, _vibrato, _randomness, false, false);
        _deadUI.DOShakeAnchorPos(_duration, _strength, _vibrato, _randomness, false, false);
    }

    
}
