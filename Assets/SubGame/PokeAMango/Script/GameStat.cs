using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStat : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private int _score = 0;
    [SerializeField]
    private float _switchTimer;
    [SerializeField]
    private GameObject _yesButton;
    [SerializeField]
    private GameObject _noButton;
    [SerializeField]
    private TextMeshProUGUI _timerText;
    [SerializeField]
    private TextMeshProUGUI _bestText;
    [SerializeField]
    private TextMeshProUGUI _endBestText;
    [SerializeField]
    private TextMeshProUGUI _endScoreText;


    private void Start()
    {

        _switchTimer = Random.Range(2f, 10f);

        
        _bestText.text = PlayerPrefs.GetInt("Best").ToString();
        Time.timeScale = 0;
        
    }
    public void GetSocre()
    {
        _score += 1;
        if (_score > PlayerPrefs.GetInt("Best"))
            PlayerPrefs.SetInt("Best", _score);
    }



    private void Update()
    {

        scoreText.text = _score.ToString();
        _endScoreText.text = _score.ToString();
        _endBestText.text = PlayerPrefs.GetInt("Best").ToString();
        _switchTimer -= Time.deltaTime;
        _timerText.text = _switchTimer.ToString();
        if (_switchTimer <= 0)
        {
            if (_yesButton.activeSelf)
            {
                _yesButton.SetActive(false);
                _noButton.SetActive(true);
            }
            else
            {
                _yesButton.SetActive(true);
                _noButton.SetActive(false);
            }
            _switchTimer = Random.Range(2f, 10f);
            //function
        }
        
    }


}
