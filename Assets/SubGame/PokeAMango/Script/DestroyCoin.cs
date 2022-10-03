using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.JungleDrum;

namespace SuperUltra.JungleDrum
{

    public class DestroyCoin : MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject, 1.5f);
        }
    }
}