using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperUltra
{
    
    public class CubeControl : MonoBehaviour
    {
        Transform _transform;
        // Start is called before the first frame update
        void Start()
        {
            _transform = GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKey(KeyCode.W))
            {
                _transform.Translate(Vector3.forward * Time.deltaTime * 10);
            }
            if(Input.GetKey(KeyCode.S))
            {
                _transform.Translate(Vector3.back * Time.deltaTime * 10);
            }
        }
    }

}
