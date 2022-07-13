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
    private float _strength = 5f;
    private int _vibrato = 80;
    private float _randomness = 10f;
    public void Shake()
    {
        _scoreText.DOShakeAnchorPos(_duration, _strength, _vibrato, _randomness, true, true);
        _bestText.DOShakeAnchorPos(_duration, _strength, _vibrato, _randomness, true, true);
        _fullTimer.DOShakeAnchorPos(_duration, _strength, _vibrato, _randomness, true, true);
        _pauseButton.DOShakeAnchorPos(_duration, _strength, _vibrato, _randomness, true, true);
        _noButton.DOShakeAnchorPos(_duration, _strength, _vibrato, _randomness, true, true);
        _deadUI.DOShakeAnchorPos(_duration, _strength, _vibrato, _randomness, true, true);
    }

    
}
