using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.GazolineRacing;

namespace SuperUltra.GazolineRacing
{

    public class CheckPoint : MonoBehaviour
    {
        [SerializeField]
        private Scroller _scroller;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("CheckPoint");
                _scroller.RandomTrack();
            }
        }
    }
}