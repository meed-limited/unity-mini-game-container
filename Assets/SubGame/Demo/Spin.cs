using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperUltra.Demo
{

    public class Spin : MonoBehaviour
    {
        Transform _transform;
        // Start is called before the first frame update
        void Start()
        {
            Debug.Log("Spin");
            _transform = GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            _transform.Rotate(Time.deltaTime * 100, Time.deltaTime * 100, Time.deltaTime * 100);
        }
    }

}
