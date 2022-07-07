using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SuperUltra.Container;

public class SceneManger : MonoBehaviour
{
    public void RestartSenece()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
   
}


