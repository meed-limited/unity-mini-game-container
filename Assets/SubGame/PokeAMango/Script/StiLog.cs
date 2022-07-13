using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StiLog : MonoBehaviour
{
    public TMPro.TextMeshProUGUI _maxStiText;
    public TMPro.TextMeshProUGUI _curStiText;
    [SerializeField]
    private int _curSti = 4;
    private int _maxSti = 5;

    private void Start()
    {
        _maxStiText.text = "Max Health: "+_maxSti.ToString();
        _curStiText.text = "Current Health: "+_curSti.ToString();
        
    }
    
    public void AddSti()
    {
        if (_curSti <5)
        {
        _curSti += 1;
        _curStiText.text = "Current Health: " + _curSti.ToString();
        }
        else
        {
            _curSti = 5;
            _curStiText.text = "Current Health: Max";
        }

    }

    public void UseSti()
    {
        if (_curSti > 0)
        {
            _curSti -= 1;
            _curStiText.text = "Current Health: " + _curSti.ToString();
        }
        else
        {
            Debug.Log("Not enough Sti");
        }
    }





}
