using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField]
    GameObject _pauseWindow;
    public void PauseGame()
    {
        Time.timeScale = 0;
        _pauseWindow.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        _pauseWindow.SetActive(false);
    }

}
