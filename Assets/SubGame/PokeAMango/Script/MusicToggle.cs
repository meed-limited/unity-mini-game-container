using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    [SerializeField]
    GameObject _music;
    [SerializeField]
    private Toggle _toggle;

    private void Start()
    {
        _toggle = gameObject.GetComponent<Toggle>();
    }
    public void MusicOnOff()
    {
        if (_toggle.isOn)
        {
            _music.SetActive(true);
        }
        else
            _music.SetActive(false);
    }
}
