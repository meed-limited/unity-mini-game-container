using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.Container;

public class BackMenu : MonoBehaviour
{

    public void Onclick()
    {
        ContainerInterface.ReturnToMenu();
    }
}
