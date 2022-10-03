using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.GazolineRacing;

namespace SuperUltra.GazolineRacing
{

    public class CarTag : MonoBehaviour
    {

        private void OnCollisionEnter(Collision collision)
        {
            // Debug.Log("collided with " + collision.gameObject.name);
        }

    }
}