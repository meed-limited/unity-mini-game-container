using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class StartButton : MonoBehaviour
{
    [SerializeField]
    GameObject _gv;
    
    
    public void OnClick()
    {
        

        Time.timeScale = 1;
        EffectControl _ef = _gv.GetComponent<EffectControl>();
        _ef.DofOff();
        gameObject.SetActive(false);
    }
}
