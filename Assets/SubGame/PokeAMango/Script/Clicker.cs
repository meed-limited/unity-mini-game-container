using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Lofelt.NiceVibrations;


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
    ParticleSystem _clickeffect;
    [SerializeField]
    HapticClip _vir;
    
 



    private void Start()
    {
        _anim = gameObject.GetComponent<Animator>();
        _sfx = gameObject.GetComponent<AudioSource>();
   
        
        
    }

    void Update()
    {
        transform.Rotate(0, 0, zvalue * Time.deltaTime);
    }

    public void OnClick()
    {
        gameman.GetComponent<GameStat>().GetSocre();
        m_camera.GetComponent<Camera>().backgroundColor = new Color(_colorValueX, _colorValueY, _colorValueZ);
        HapticController.Play(_vir);
        
        var _coin = Instantiate(_clickeffect, transform.position, Quaternion.identity);
        _coin.transform.localScale = new Vector3(0.6f,0.7f,1);
        ColorChange();
        

        _anim.SetTrigger("AniPlayer");

        //Debug.Log("Clicked");

        if (_sfx != null)
        {
            _sfx.Play();
        }
            

    }

    private void ColorChange()
    {
        if (_colorValueX >= 1 && _colorValueY < 1 && _colorValueZ < 1)
        {
            Debug.Log("C1");
            _colorValueY -= 0.02f;
            _colorValueZ += 0.02f;
        }
        else if (_colorValueX <= 0.45 && _colorValueY < 1 && _colorValueZ >= 1)
        {
            _colorValueY += 0.02f;
            Debug.Log("C2");
        }
        else if (_colorValueX >= 1 && _colorValueY < .8 && _colorValueZ > .8)
        {
            _colorValueZ = 0.8f;
            _colorValueX -= 0.02f;

            Debug.Log("C3");
        }
        else
        {
            _colorValueX -= 0.02f;
            Debug.Log("C4");

        }
    }


}
