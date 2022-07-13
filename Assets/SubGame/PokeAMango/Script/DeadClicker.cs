using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Animations;
using UnityEngine.UI;
using Lofelt.NiceVibrations;

public class DeadClicker : MonoBehaviour
{
    [SerializeField]
    GameObject _deadUI;
    private float zvalue = 50;
    AudioSource _endSfx;
    [SerializeField]
    GameObject _gv;
    [SerializeField]
    GameObject _gm;
    [SerializeField]
    GameObject _player;
    GameStat _gs;
    Timer _timer;
    [SerializeField]
    HapticClip _vir;
    [SerializeField]
    Clicker _clicker;
    [SerializeField]
    bool _aniBlocked;

    private void Start()
    {
        _endSfx = gameObject.GetComponent<AudioSource>();
        _gs = _gm.GetComponent<GameStat>();
        _timer = _gm.GetComponent<Timer>();
        _clicker = _clicker.GetComponent<Clicker>();
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

    IEnumerator EndGamee()
    {
        yield return new WaitForSeconds(2.5f);
        _deadUI.SetActive(true);
        EffectControl _ef = _gv.GetComponent<EffectControl>();
        _ef.DofOn();
    }

    public void OnClick()
    {
        if (SceneManager.GetActiveScene().name == "Tourment")
        {
            HapticController.Play(_vir);
            PtReduce();

        }
        else
        {
            HapticController.Play(_vir);
            GameEnd();

        }
        _aniBlocked = true;
        Invoke("AniUnlock", 4f);
    }

    private void AniUnlock()
    {
        _aniBlocked = false;
    }
    public void GameEnd()
    {
        
        _player.GetComponent<Animator>().SetTrigger("Dead");
        gameObject.GetComponent<Button>().interactable = false;
        _endSfx.Play();
        _timer.enabled = false;
        _gs._isSwitching = false;

        StartCoroutine(EndGamee());

    }

    public void PtReduce()
    {
        if (!_aniBlocked)
            _player.GetComponent<Animator>().SetTrigger("running");
        _player.transform.DOMoveX(-2f, 0.5f, false);
        _clicker._moveValue = -2f;
        //_player.transform.position = new Vector3(-2, 1.616f, 10f);
        if (_gs._score >= 50)
            _gs._score -= 50;
        else
            _gs._score = 0;
    }
}
