using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SuperUltra
{
    
    public class PaintColor : MonoBehaviour
    {
        MeshRenderer _meshRenderer;
        // Start is called before the first frame update
        void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void ChangeColor()
        {
            _meshRenderer.material.color = new Color(Random.value, Random.value, Random.value);
        }

    }

}