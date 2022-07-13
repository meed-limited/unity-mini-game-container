using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField]
    GameObject _coin;
    public int _l3p = 0;
    [SerializeField]
    Animator _chestani;
    public TMPro.TextMeshProUGUI L3p;
    Vector3 _coinSpawn;
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Tourment")
            _chestani = _chestani.GetComponent<Animator>();
        _coinSpawn = new Vector3(transform.position.x, 1.8f, 4.5f);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        _l3p += 1;
        other.transform.position = new Vector3(-2f, 1.616f, 10.1f);
        Rigidbody coin = Instantiate(_coin, _coinSpawn,Quaternion.identity).GetComponent<Rigidbody>();
        coin.DOMoveY(4f, 0.5f, false);
        L3p.text = "L3p: " + _l3p.ToString();
        }
        if (SceneManager.GetActiveScene().name == "Tourment")
            _chestani.SetTrigger("Open");
    }

    
}
