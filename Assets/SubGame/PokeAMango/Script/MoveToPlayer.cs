using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SuperUltra.JungleDrum;

namespace SuperUltra.JungleDrum
{
    public class MoveToPlayer : MonoBehaviour
    {
        [SerializeField]
        GameObject player;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            gameObject.transform.DOMoveX(player.transform.position.x, 0.2f, false);
            Invoke("Destroyit", 5f);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                gameObject.GetComponent<Rigidbody>().AddForce(transform.right);
                Debug.Log("go back!");
            }

        }

        private void Destroyit()
        {
            Destroy(gameObject);
        }
    }
}