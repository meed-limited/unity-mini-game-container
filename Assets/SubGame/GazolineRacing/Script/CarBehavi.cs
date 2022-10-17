using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehavi : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.tag = "Car";
    }

    private void OnDisable()
    {
        gameObject.tag = "Untagged";
    }
}
