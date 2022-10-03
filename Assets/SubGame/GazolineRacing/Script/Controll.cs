using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.GazolineRacing;

namespace SuperUltra.GazolineRacing
{

    public class Controll : MonoBehaviour
    {
        private Rigidbody _rb;

        private void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody>();
        }
        private void Update()
        {
            if (Input.GetKeyDown("w"))
                transform.position += Vector3.forward * 100 * Time.deltaTime;
            else if (Input.GetKeyDown("s"))
                transform.position += Vector3.back * 100 * Time.deltaTime;
            else if (Input.GetKeyDown("a"))
                transform.position += Vector3.left * 100 * Time.deltaTime;
            else if (Input.GetKeyDown("d"))
                transform.position += Vector3.right * 100 * Time.deltaTime;
        }
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.gameObject.name);
        }
    }
}