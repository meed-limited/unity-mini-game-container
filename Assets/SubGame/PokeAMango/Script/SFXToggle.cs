using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXToggle : MonoBehaviour
{
    [SerializeField]
    GameObject _clicker;
    [SerializeField]
    GameObject _badClicker;
    private Toggle _toggle;
    AudioSource _sfx;
    AudioSource _badSfx;

    private void Start()
    {
        _toggle = gameObject.GetComponent<Toggle>();
        _sfx = _clicker.GetComponent<AudioSource>();
        _badSfx = _badClicker.GetComponent<AudioSource>();

    }
    public void SFXOnOff()
    {
        if (_toggle.isOn)
        {
            if (_sfx != null || _badSfx != null)
            {
            _sfx.enabled = true;
            _badSfx.enabled = true;

            }
        }
        else
            _sfx.enabled = false;
            _badSfx.enabled = false;
    }
}
