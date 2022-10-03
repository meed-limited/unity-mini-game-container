using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.JungleDrum;

namespace SuperUltra.JungleDrum
{

    public class EffectDestroy : MonoBehaviour
    {
        private void Start()
        {
            Invoke("Destroyit", 2f);
        }

        private void Destroyit()
        {
            Destroy(gameObject);
        }
    }
}