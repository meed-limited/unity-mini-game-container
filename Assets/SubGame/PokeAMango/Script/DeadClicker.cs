using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadClicker : MonoBehaviour
{
    [SerializeField]
    GameObject _deadUI;
    private float zvalue = 50;
    AudioSource _endSfx;

    private void Start()
    {
        _endSfx = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Rotate(0, 0, zvalue * Time.deltaTime);

    }

    public void GameEnd()
    {
        Time.timeScale = 0;
        _deadUI.SetActive(true);
        _endSfx.Play();

    }
}
