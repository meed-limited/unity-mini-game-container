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
    IEnumerator PauseGame()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;

    }
    public void GameEnd()
    {
        _deadUI.SetActive(true);
        _endSfx.Play();
        StartCoroutine(PauseGame());

    }
}
