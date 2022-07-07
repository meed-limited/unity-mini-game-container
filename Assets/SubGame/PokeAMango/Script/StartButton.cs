using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class StartButton : MonoBehaviour
{
    [SerializeField]
    GameObject _Volume;
    Volume _v;
    DepthOfField _df;

    private void Start()
    {
        _v = _Volume.GetComponent<Volume>();
        //_v.profile.TryGet(out _df);
        
    }
    public void OnClick()
    {
        

        Time.timeScale = 1;
        //_df.active = false;
        gameObject.SetActive(false);
    }
}
