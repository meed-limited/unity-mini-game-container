using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.GazolineRacing;

namespace SuperUltra.GazolineRacing
{

    public class DestoryItself : MonoBehaviour
    {
        private void OnDisable()
        {
            GetComponent<AudioSource>().enabled = false;
        }
    }
}