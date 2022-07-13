using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class StiTimer : MonoBehaviour
{
    public TMPro.TextMeshProUGUI timerText;
    public TMPro.TextMeshProUGUI _maxSti;
    public TMPro.TextMeshProUGUI _curSti;
    [SerializeField]
    float _curHealth;


    private void Start()
    {
        Debug.Log("Game Start");
        string dateQuitString = PlayerPrefs.GetString("dateQuit", "");
        if (!dateQuitString.Equals(""))
        {
            DateTime dateQuit = DateTime.Parse(dateQuitString);
            DateTime dateNow = DateTime.Now;

            if (dateNow > dateQuit)
            {
                TimeSpan timespan = dateNow - dateQuit;
                Debug.Log("Quit for " + timespan.TotalSeconds);
            }

            PlayerPrefs.SetString("dateQuit", "");
        }
    }

    private void OnApplicationQuit()
    {
        DateTime dateQuit = DateTime.Now;
        PlayerPrefs.SetString("dateQuit", dateQuit.ToString());
        Debug.Log("App Quit!");
        Debug.Log("Quit Time: "+dateQuit);
    }

    private void OnApplicationPause()
    {
        Debug.Log("App Pasued!");
    }
}