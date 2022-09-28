using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperUltra.Container;
using SuperUltra.JungleDrum;

namespace SuperUltra.JungleDrum
{

    public class BackMenu : MonoBehaviour
    {

        public void Onclick()
        {
            ContainerInterface.ReturnToMenu();
        }
    }
}
