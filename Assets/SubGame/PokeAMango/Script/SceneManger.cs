using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SuperUltra.Container;

public class SceneManger : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("awake");
        Time.timeScale = 0;
    }

    public void RestartSenece()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Tourment()
    {
        SceneManager.LoadScene(2);
    }

    public void SuddenDead()
    {
        SceneManager.LoadScene(1);
    }
   
}


