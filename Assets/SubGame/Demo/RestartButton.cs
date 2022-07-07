using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.Container;

namespace SuperUltra.Demo
{

    public class RestartButton : MonoBehaviour
    {
        public void Return()
        {
            ContainerInterface.ReturnToMenu();
        }
    }

}
