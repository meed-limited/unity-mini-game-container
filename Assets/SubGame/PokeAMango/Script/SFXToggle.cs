using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SuperUltra.Container;

public class SFXToggle : MonoBehaviour
{
    [SerializeField]
    GameObject _clicker;
    [SerializeField]
    GameObject _badClicker;
    AudioSource _sfx;
    AudioSource _badSfx;


    private void OnEnable()
    {
        ContainerInterface.OnEffectVolumeChange += SFXOnOff;
    }

    private void OnDisable()
    {
        ContainerInterface.OnEffectVolumeChange -= SFXOnOff;
    }
    private void Start()
    {
        _sfx = _clicker.GetComponent<AudioSource>();
        _badSfx = _badClicker.GetComponent<AudioSource>();

    }
    public void SFXOnOff(bool _inOn)
    {
        if (_inOn)
        {
            if (_sfx != null || _badSfx != null)
            {
                _sfx.enabled = true;
                _badSfx.enabled = true;

            }
        }
        else
        {
            _sfx.enabled = false;
            _badSfx.enabled = false;

        }
    }
}
