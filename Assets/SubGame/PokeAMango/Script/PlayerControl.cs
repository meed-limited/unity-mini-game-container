using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SuperUltra.JungleDrum;
namespace SuperUltra.JungleDrum
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField]
        Scroller _scroller;
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("dog hit!");
                RollAniPlay();
                gameObject.transform.DOMoveX(-2f, 0.5f, false);
                _scroller.StartCoroutine(_scroller.MoveBack());
            }
        }

        private void RollAniPlay()
        {
            gameObject.GetComponent<Animator>().SetTrigger("running");
        }
    }
}