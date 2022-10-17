using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.GazolineRacing;
using DG.Tweening;
using UnityEngine.UI;

public class CoinMovemen : MonoBehaviour
{
    private CharacterControllerS _ccs;
    private GameObject _coin;

    private void Start()
    {
        _coin = GameObject.FindGameObjectWithTag("Coin");
        _ccs = GameObject.Find("LevelGenerator").GetComponent<CharacterControllerS>();
        transform.DOMove(_coin.GetComponent<RectTransform>().position , 1f).OnComplete(() => gameObject.SetActive(false));

    }
    //new Vector3(392.10f, 2420.45f, 0)

}
